using FocalPoint.Data;
using Visum.Services.Mobile.Entities;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{
    public class QuickOrderDetailsViewModel : ThemeBaseViewModel
    {
        public ICommand OpenPhoneCmd { get; }

        private int StoreID = 1;
        private string Search = "%";
        private string OutDate = "2020-01-01T18:25:00.000";
        private string DueDate = "2020-01-01T18:25:00.000";
        private Int16 SearchIn = 1;
        private char SearchType = '0';
        private Int16 SearchFor = 1;
        private int StartIdx = 0;
        private int MaxCnt = 100;

        IList<Customer> customers;
        ObservableCollection<AvailabilityRent> recent;
        public ObservableCollection<AvailabilityRent> Recent
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
        private Order currentOrder = new Order();
        public Order CurrentOrder
        {
            get { return currentOrder; }
            set
            {
                if (currentOrder != value)
                {
                    currentOrder = value;
                    OnPropertyChanged("CurrentOrder");
                }
            }
        }

        internal void GetSearchedCustomersInfo(string text)
        {
            //update searchText
            if (text == null)
                text = "%";
            Search = text;
            List<AvailabilityRent> customersCntAndList = null;
            try { 
            Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "Availability/Rentals/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
            var stringContent = new StringContent(
                                      JsonConvert.SerializeObject(new { Search, OutDate, DueDate, StoreID, SearchIn, SearchType, SearchFor}),
                                      Encoding.UTF8,
                                      "application/json");
            //ClientHTTP.DefaultRequestHeaders.Add("Token", "987919a1-b105-4c16-99d8-9c8ec2b81dcf");//"3d2ad6f3-8f4a-4c47-8e8b-69f0b1a7ec08"); 70e2aad8-6216-48cc-ab13-3439970a189a
            var response = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                customersCntAndList = JsonConvert.DeserializeObject<List<AvailabilityRent>>(content);
                //StartIdx = customersCntAndList.TotalCnt;
                if (recent == null)
                {
                    Recent = new ObservableCollection<AvailabilityRent>(customersCntAndList);
                }
                else
                {
                    Recent.Clear();
                    foreach (var customer in customersCntAndList)
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
        public QuickOrderDetailsViewModel()
        {
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
            // clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
        }


        HttpClient clientHttp;
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        List<Customer> custList = new List<Customer>();

        void ExecuteLoadMoreCommand()
        {
        }
        OrderUpdate orderUpdate;

        internal OrderUpdate AddItem(AvailabilityRent selItem, decimal numOfItems, Order curOrder, OrderUpdate myOrderUpdate, out QuestionFaultExceptiom result)
        {
            // success no questions needed
            result = null;
            try
            {
                orderUpdate = new OrderUpdate();
                //update searchText
                OrderAddItem RentalItem = new OrderAddItem();
                RentalItem.OrderNo = curOrder.OrderNo;
                RentalItem.AvailItem = selItem.AvailItem;
                RentalItem.Quantity = numOfItems;
                if (myOrderUpdate != null)
                    RentalItem.Answers = myOrderUpdate.Answers;
                //RentalItem.Serials.Add("hell bob");
                // KIRK REM
                //foreach (var kvp in RentalItem.Answers)
                //    RentalItem.Questions.Add(new QuestionAnswer { Answer = kvp.Value, Code = kvp.Key });

                //foreach (var listItem in RentalItem.Questions)
                //    RentalItem.Answers[listItem.Code] = listItem.Answer;
                // END KIRK
                //MerchItem.Serials
                OrderUpdate OrderToUpDate = new OrderUpdate();

                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "OrderAddRental/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                var stringContent = new StringContent(
                                          JsonConvert.SerializeObject(new { RentalItem }),
                                          Encoding.UTF8,
                                          "application/json");
                var responseOrderUpdate = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
                if (responseOrderUpdate.IsSuccessStatusCode)
                {
                    string orderContent = responseOrderUpdate.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };

                    orderUpdate = JsonConvert.DeserializeObject<OrderUpdate>(orderContent, settings);
                    orderUpdate.Answers.Clear();
                    return orderUpdate;
                }
                if (responseOrderUpdate.StatusCode == System.Net.HttpStatusCode.ExpectationFailed)
                {
                    previousItem = RentalItem;
                    string readErrorContent = responseOrderUpdate.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };

                    result = JsonConvert.DeserializeObject<QuestionFaultExceptiom>(readErrorContent, settings);      
                }
                return orderUpdate;
            }
            catch (Exception ex)
            {
                result = null;
                return null;
            }
        }

        private OrderAddItem previousItem = new OrderAddItem();
        private Order editingOrder = new Order();
        private string exceptionMessage = "";
        internal List<string> GetSerials(AvailabilityMerch selItem)
        {
            throw new NotImplementedException();
        }
    }
}