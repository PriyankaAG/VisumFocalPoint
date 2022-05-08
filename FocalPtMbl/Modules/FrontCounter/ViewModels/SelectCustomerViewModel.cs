using FocalPoint.Data;
using Visum.Services.Mobile.Entities;
using FocalPoint.Modules.FrontCounter.Views;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{
    public class SelectCustomerViewModel : ThemeBaseViewModel
    {
        public ICommand OpenPhoneCmd { get; }
        readonly Customer repository;
        IList<Customer> customers;
        ObservableCollection<Customer> recent;
        public ObservableCollection<string> Greetings { get; set; }
        public ObservableCollection<Customer> Recent
        {
            get => this.recent;
            private set
            {
                this.recent = value;
                OnPropertyChanged(nameof(Recent));
            }
        }
        public IList<Customer> CustList
        {
            get { return customers; }
            set
            {
                if (customers != value)
                {
                    customers = value;
                    OnPropertyChanged("CustList");
                }
            }
        }
        bool isRefreshing = false;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    OnPropertyChanged("IsRefreshing");
                }
            }
        }

        internal void GetSearchedCustomersInfo(string text)
        {
            //update searchText
            try { 
            SearchText = text;
            StartIdx = 0;
            Customers customersCntAndList = null;

            Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "Customers/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
            var stringContent = new StringContent(
                                      JsonConvert.SerializeObject(new { StoreID, SearchText, StartIdx, MaxCnt }),
                                      Encoding.UTF8,
                                      "application/json");
            //ClientHTTP.DefaultRequestHeaders.Add("Token", "987919a1-b105-4c16-99d8-9c8ec2b81dcf");//"3d2ad6f3-8f4a-4c47-8e8b-69f0b1a7ec08"); 70e2aad8-6216-48cc-ab13-3439970a189a
            var response = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                customersCntAndList = JsonConvert.DeserializeObject<Customers>(content);
                StartIdx = customersCntAndList.TotalCnt;
                if (recent == null)
                {
                    Recent = new ObservableCollection<Customer>(customersCntAndList.List);
                }
                else
                {
                    Recent.Clear();
                    foreach (var customer in customersCntAndList.List)
                    {
                        Recent.Add(customer);
                    }
                }
            }
            }
            catch (Exception ex)
            {

            }
        }

        internal OrderUpdate UpdateCust(Customer selectedCustomer)
        {
            try
            {
                if (CurrentOrder != null)
                {
                    CurrentOrder.Customer = selectedCustomer;
                    CurrentOrder.OrderCustNo = selectedCustomer.CustomerNo;
                    var Update = orderUpdate;
                    Update.Order = CurrentOrder;

                    Uri uriUpdateCust = new Uri(string.Format(DataManager.Settings.ApiUri + "Order/"));
                    var stringContent = new StringContent(
                        JsonConvert.SerializeObject(new { Update }),
                          Encoding.UTF8,
                          "application/json");
                    var responseOrderUpdate = ClientHTTP.PutAsync(uriUpdateCust, stringContent).GetAwaiter().GetResult();
                    if (responseOrderUpdate.IsSuccessStatusCode)
                    {
                        //Check for messages
                        orderUpdateRefresh = new OrderUpdate();
                        string orderContent = responseOrderUpdate.Content.ReadAsStringAsync().Result;


                        var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };

                        orderUpdateRefresh = JsonConvert.DeserializeObject<OrderUpdate>(orderContent, settings);
                        orderUpdateRefresh = JsonConvert.DeserializeObject<OrderUpdate>(orderContent, settings);

                        //orderUpdate = JsonConvert.DeserializeObject<OrderUpdate>(orderContent);
                        //check if empty result
                        if (orderUpdateRefresh != null && orderUpdateRefresh.Order != null)
                        {
                            CurrentOrder = orderUpdateRefresh.Order;
                            if (orderUpdateRefresh.Answers != null && orderUpdateRefresh.Answers.Count > 0)
                            {
                                orderUpdateRefresh.Answers.Clear();
                                return orderUpdateRefresh;
                            }
                        }
                        else
                            throw new Exception("Order customer not changed");
                    }
                    else if(responseOrderUpdate.StatusCode == System.Net.HttpStatusCode.ExpectationFailed)
                    {
                        string readErrorContent = responseOrderUpdate.Content.ReadAsStringAsync().Result;
                        var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };

                        QuestionFaultExceptiom questionFaultExceptiom = JsonConvert.DeserializeObject<QuestionFaultExceptiom>(readErrorContent, settings);
                        //exceptionMessage = questionFaultExceptiom.Message;

                        orderUpdateRefresh.Answers.Add(new QuestionAnswer(questionFaultExceptiom.Code, questionFaultExceptiom.Message));
                        return orderUpdateRefresh;
                        //orderUpdate.Answers.
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is QuestionFaultExceptiom)
                {
                    //return message error
                }
                if (ex.InnerException is string)
                {

                    //return message error
                }
                // back out order failed to create
            }
            return orderUpdate;
        }

        ICommand loadMoreCommand = null;
        public ICommand LoadMoreCommand
        {
            get { return loadMoreCommand; }
            set
            {
                if (loadMoreCommand != value)
                {
                    loadMoreCommand = value;
                    OnPropertyChanged("LoadMoreCommand");
                }
            }
        }
        public SelectCustomerViewModel(Order curOrder)
        {
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
            // clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
            Greetings = new ObservableCollection<string>();

            //MessagingCenter.Subscribe<MainPage>(this, "Hi", (sender) =>
            //{
            //    Greetings.Add("Hi");
            //});
            CurrentOrder = curOrder;
            MessagingCenter.Subscribe<SelectCustomerView, string>(this, "Hi", (sender, arg) =>
            {
                Greetings.Add("Hi " + arg);
            });
        }

        HttpClient clientHttp;
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        List<Customer> custList = new List<Customer>();

        private int StoreID = 0;
        private string SearchText = "";
        private int StartIdx = 0;
        private int MaxCnt = 100;

        public Customers GetCustomersInfo()
        {
            Customers customersCntAndList = null;
            Customer customersist = null;
            try
            {

                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "Customers/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                var stringContent = new StringContent(
                                          JsonConvert.SerializeObject(new { StoreID, SearchText, StartIdx, MaxCnt }),
                                          Encoding.UTF8,
                                          "application/json");
                // var productValue = new ProductInfoHeaderValue("FocalPoint Mobile\3.0.0"); //"FocalPoint Mobile\\3.0.0"
                // var commentValue = new ProductInfoHeaderValue("(+http://www.example.com/ScraperBot.html)");
                // ClientHTTP.DefaultRequestHeaders.UserAgent.Add(productValue); 05fe29ff-6640-487b-9331-6b5759851bca

                //"3d2ad6f3-8f4a-4c47-8e8b-69f0b1a7ec08"); 70e2aad8-6216-48cc-ab13-3439970a189a
                var response = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    // var asdf  = JsonConvert.DeserializeObject(content);
                    customersCntAndList = JsonConvert.DeserializeObject<Customers>(content);
                    StartIdx = customersCntAndList.TotalCnt;
                    if (recent == null)
                    {
                        Recent = new ObservableCollection<Customer>(customersCntAndList.List);
                    }
                    else
                    {
                        foreach (var customer in customersCntAndList.List)
                        {
                            Recent.Add(customer);
                        }
                    }

                    //var custList = JsonConvert.DeserializeObject<List<FocalPoint.Data.DataLayer.Customer>(content);
                    // Recent = (ObservableCollection<FocalPoint.Data.DataLayer.Customer>)CustList;
                }
                return customersCntAndList;
            }
            catch (Exception ex)
            { return customersCntAndList; }
        }
        void ExecuteLoadMoreCommand()
        {
        }
        private Customer selectedCustomer;
        public Customer SelectedCustomer
        {
            get { return selectedCustomer; }
            set
            {
                if (selectedCustomer != value)
                {
                    selectedCustomer = value;
                    OnPropertyChanged(nameof(SelectedCustomer));
                }
            }
        }
        private string selectedCustomerName;
        private OrderUpdate orderUpdate = new OrderUpdate();
        public OrderUpdate orderUpdateRefresh
        {
            get { return orderUpdate; }
            set
            {
                if (orderUpdate != value)
                {
                    orderUpdate = value;
                    OnPropertyChanged(nameof(orderUpdateRefresh));
                }
            }
        }

        public string SelectedCustomerName
        {
            get { return SelectedCustomer.CustomerName; }
            set
            {
                if (SelectedCustomer.CustomerName != value)
                {
                    SelectedCustomer.CustomerName = value;
                    OnPropertyChanged(nameof(SelectedCustomerName));
                }
            }
        }

        public Order CurrentOrder { get; private set; }

        public void GetCustomerInfo()
        {
            try { 
            Uri uri = new Uri(string.Format("https://10.0.2.2:56883/Mobile/V1/Customer/1"));

            var stringContent = new StringContent(
                          JsonConvert.SerializeObject(new { SelectedCustomer }),
                          Encoding.UTF8,
                          "application/json");

            var response = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {

            }
            }
            catch (Exception ex)
            {

            }
        }
        private void GetCustomerBalance()
        {
            try { 
            Uri uri = new Uri(string.Format("https://10.0.2.2:56883/Mobile/V1/CustomerBalance/1"));
            var stringContent = new StringContent(
                          JsonConvert.SerializeObject(new { Recent }),
                          Encoding.UTF8,
                          "application/json");
            var response = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {

            }
            }
            catch (Exception ex)
            {

            }
        }
        private void GetCustomerEmail()
        {
            Uri uri = new Uri(string.Format("https://10.0.2.2:56883/Mobile/V1/CustomerEmails/"));
        }
    }
}
