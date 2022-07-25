using FocalPoint.Data;
using Visum.Services.Mobile.Entities;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FocalPoint.Modules.CustomerRelations.ViewModels
{
    public class CustomerSimpleViewModel : ThemeBaseViewModel
    {
        //Contact selectedContact;
        //public List<Contact> Data { get; }
        //public Contact SelectedContact
        //{
        //    get { return selectedContact; }
        //    set
        //    {
        //        if (selectedContact != value)
        //        {
        //            selectedContact = value;
        //        }
        //    }
        //}
        public ICommand OpenPhoneCmd { get; }
        readonly Visum.Services.Mobile.Entities.Customer repository;
        IList<Visum.Services.Mobile.Entities.Customer> customers;
        ObservableCollection<Visum.Services.Mobile.Entities.Customer> recent;
        public ObservableCollection<Visum.Services.Mobile.Entities.Customer> Recent
        {
            get => this.recent;
            private set
            {
                this.recent = value;
                OnPropertyChanged(nameof(Recent));
            }
        }
        public IList<Visum.Services.Mobile.Entities.Customer> CustList
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
            SearchText = text;
            StartIdx = 0;
            Visum.Services.Mobile.Entities.Customers customersCntAndList = null;
            try
            {
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
        public CustomerSimpleViewModel()
        {
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
            Task.Run(() =>
            {
                _ = GetCustomersInfo();
            });
        }

        private HttpClient PreparedClient()
        {
            HttpClientHandler handler = new HttpClientHandler();

            //not sure about this one, but I think it should work to allow all certificates:
            handler.ServerCertificateCustomValidationCallback += (sender, cert, chaun, ssPolicyError) =>
            {
                return true;
            };


            HttpClient client = new HttpClient(handler);
            //ClientHTTP.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("3d2ad6f3-8f4a-4c47-8e8b-69f0b1a7ec08");

            // client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(""); //DataManager.Settings.UserToken
            return client;
        }

        HttpClient clientHttp = new HttpClient();
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        List<Customer> custList = new List<Customer>();

        private int StoreID = 0;
        private string SearchText = "";
        private int StartIdx = 0;
        private int MaxCnt = 100;

        public async Task<Customers> GetCustomersInfo()
        {
            Indicator = true;
            Customers customersCntAndList = null;
            Customer customersist = null;
            StartIdx = 0;
            StoreID = DataManager.Settings.HomeStore;
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
                var response = await ClientHTTP.PostAsync(uri, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    // var asdf  = JsonConvert.DeserializeObject(content);
                    customersCntAndList = JsonConvert.DeserializeObject<Customers>(content);
                    StartIdx = customersCntAndList.TotalCnt;
                    Recent = new ObservableCollection<Customer>(customersCntAndList.List);

                    //if (recent == null)
                    //{
                    //    Recent = new ObservableCollection<Customer>(customersCntAndList.List);
                    //}
                    //else
                    //{
                    //    foreach (var customer in customersCntAndList.List)
                    //    {
                    //        Recent.Add(customer);
                    //    }
                    //}

                    //var custList = JsonConvert.DeserializeObject<List<FocalPoint.Data.DataLayer.Customer>(content);
                    // Recent = (ObservableCollection<FocalPoint.Data.DataLayer.Customer>)CustList;
                }
                return customersCntAndList;
            }
            catch (Exception ex)
            { return customersCntAndList; }
            finally
            {
                Indicator = false;
            }
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
                }
            }
        }
        public void GetCustomerInfo()
        {
            try
            {
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
        public async Task<CustomerBalance> GetCustomerBalance(int custNo)
        {
            try
            {
                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "CustomerBalance/" + custNo));
                var response = await ClientHTTP.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<CustomerBalance>(content);
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        private void GetCustomerEmail()
        {
            Uri uri = new Uri(string.Format("https://10.0.2.2:56883/Mobile/V1/CustomerEmails/"));
        }

        public class Contact
        {
            string name;
            public string Name
            {
                get => this.name;
                set
                {
                    this.name = value;
                }
            }
            string email;
            public string Email
            {
                get => this.email;
                set
                {
                    this.email = value;
                }
            }

            public Contact(string name, string phone, string email)
            {
                Name = name;
                Phone = phone;
                Email = email;
            }
            public string Phone { get; set; }
        }
    }
}
