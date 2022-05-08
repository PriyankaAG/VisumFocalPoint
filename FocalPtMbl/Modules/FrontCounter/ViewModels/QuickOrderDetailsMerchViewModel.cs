using FocalPoint.Data;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{
    public class QuickOrderDetailsMerchViewModel : ThemeBaseViewModel
    {
        public ICommand OpenPhoneCmd { get; }

        private int StoreID = 1;
        private string Search = "%";
        private string OutDate = "2020-01-01T18:25:00.000";
        private string DueDate = "2020-01-01T18:25:00.000";
        private Int16 SearchIn = 1;
        private char SearchType = '0';
        private Int16 SearchFor = 1;

        ObservableCollection<AvailabilityMerch> recent;
        public ObservableCollection<AvailabilityMerch> Recent
        {
            get => this.recent;
            private set
            {
                this.recent = value;
                OnPropertyChanged(nameof(Recent));
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

        internal void GetSearchedMerchInfo(string text)
        {
            try { 
            //update searchText
            Search = text;
            List<AvailabilityMerch> merchCntAndList = null;

            Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "Availability/Merchandise/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
            var stringContent = new StringContent(
                                      JsonConvert.SerializeObject(new { Search, SearchIn }),
                                      Encoding.UTF8,
                                      "application/json");
            //ClientHTTP.DefaultRequestHeaders.Add("Token", "987919a1-b105-4c16-99d8-9c8ec2b81dcf");//"3d2ad6f3-8f4a-4c47-8e8b-69f0b1a7ec08"); 70e2aad8-6216-48cc-ab13-3439970a189a
            var response = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                    merchCntAndList = JsonConvert.DeserializeObject<List<AvailabilityMerch>>(content);
                //StartIdx = customersCntAndList.TotalCnt;
                if (recent == null)
                {
                    Recent = new ObservableCollection<AvailabilityMerch>(merchCntAndList);
                }
                else
                {
                    Recent.Clear();
                    foreach (var customer in merchCntAndList)
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
        public QuickOrderDetailsMerchViewModel()
        {
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
            // clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
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


        HttpClient clientHttp;
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        OrderUpdate orderUpdate;

        internal OrderUpdate AddItem(AvailabilityMerch selItem, decimal numOfItems, Order curOrder)
        {
            try
            {
                orderUpdate = new OrderUpdate();
                //update searchText
                OrderAddItem MerchItem = new OrderAddItem();
                MerchItem.OrderNo = curOrder.OrderNo;
                MerchItem.AvailItem = selItem.AvailItem;
                MerchItem.Quantity = numOfItems;
               // MerchItem.Answers.Add(1005, "Test");
                //MerchItem.Answers.Add(1004, "Test");
                //MerchItem.Serials
                OrderUpdate OrderToUpDate = new OrderUpdate();

                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "OrderAddMerchandise/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                var Temp = JsonConvert.SerializeObject(new { MerchItem });
                var stringContent = new StringContent(
                                          JsonConvert.SerializeObject(new { MerchItem }),
                                          Encoding.UTF8,
                                          "application/json");
                var responseOrderUpdate = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
                if (responseOrderUpdate.IsSuccessStatusCode)
                {
                    string orderContent = responseOrderUpdate.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };

                   // orderUpdate = JsonConvert.DeserializeObject<OrderUpdate>(orderContent, settings);
                    orderUpdate = JsonConvert.DeserializeObject<OrderUpdate>(orderContent, settings);
                    orderUpdate.Answers.Clear();
                    return orderUpdate;
                }
                if(responseOrderUpdate.StatusCode == System.Net.HttpStatusCode.ExpectationFailed)
                {
                    string readErrorContent = responseOrderUpdate.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };

                    QuestionFaultExceptiom questionFaultExceptiom = JsonConvert.DeserializeObject<QuestionFaultExceptiom>(readErrorContent, settings);
                    exceptionMessage = questionFaultExceptiom.Message;

                    orderUpdate.Answers.Add(new QuestionAnswer(questionFaultExceptiom.Code, ""));
                    //orderUpdate.Answers.

                    throw new Exception(questionFaultExceptiom.Code.ToString());
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "1005")
                    return new OrderUpdate();
                else return null;
            }
            return null;
        }
        private string exceptionMessage="";
        internal List<string> GetSerials(AvailabilityMerch selItem)
        {
            throw new NotImplementedException();
        }
    }
}
