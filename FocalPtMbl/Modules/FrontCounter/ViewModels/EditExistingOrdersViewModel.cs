using FocalPoint.Data;
using Visum.Services.Mobile.Entities;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{
    public class EditExistingOrdersViewModel : ThemeBaseViewModel
    {
        ObservableCollection<Order> openOrders = new ObservableCollection<Order>();
        private ObservableCollection<Order> OpenOrders_Original = new ObservableCollection<Order>();
        public ObservableCollection<Order> OpenOrders
        {
            get => this.openOrders;
            private set
            {
                this.openOrders = value;
                OnPropertyChanged(nameof(OpenOrders));
            }
        }
        ObservableCollection<Order> openReserv = new ObservableCollection<Order>();
        private ObservableCollection<Order> OpenReserv_Original = new ObservableCollection<Order>();
        public ObservableCollection<Order> OpenReserv
        {
            get => this.openReserv;
            private set
            {
                this.openReserv = value;
                OnPropertyChanged(nameof(OpenReserv));
            }
        }
        ObservableCollection<Order> openQuote = new ObservableCollection<Order>();
        private ObservableCollection<Order> OpenQuote_Original = new ObservableCollection<Order>();
        public ObservableCollection<Order> OpenQuote
        {
            get => this.openQuote;
            private set
            {
                this.openQuote = value;
                OnPropertyChanged(nameof(OpenQuote));
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

        internal void GetSearchedOrdersInfo(string text, int orderType, bool isNewSearch)
        {
            //  Function Orders(ByVal OrderType As Integer, ByVal SearchText As String, ByVal StartIdx As Integer, ByVal MaxCnt As Integer) As Orders
            try
            {
                int StartIdx = 0;
                int MaxCnt = 100;
                //update searchText
                int OrderType = orderType;
                SearchText = text;

                if (OrderType == 1)
                {
                    if (isNewSearch)
                    {
                        StartIdxOrd = 0;
                        MaxCntOrd = 100;
                    }
                    StartIdx = StartIdxOrd;
                    MaxCnt = MaxCntOrd;

                }
                if (OrderType == 2)
                {
                    if (isNewSearch)
                    {
                        StartIdxRes = 0;
                        MaxCntRes = 100;
                    }
                    StartIdx = StartIdxRes;
                    MaxCnt = MaxCntRes;
                }
                if (OrderType == 3)
                {
                    if (isNewSearch)
                    {
                        StartIdxQuote = 0;
                        MaxCntQuote = 100;
                    }
                    StartIdx = StartIdxQuote;
                    MaxCnt = MaxCntQuote;
                }

                Orders orderCntAndList = null;

                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "Orders/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                var stringContent = new StringContent(
                                          JsonConvert.SerializeObject(new { OrderType, SearchText, StartIdx, MaxCnt }),
                                          Encoding.UTF8,
                                          "application/json");
                //ClientHTTP.DefaultRequestHeaders.Add("Token", "987919a1-b105-4c16-99d8-9c8ec2b81dcf");//"3d2ad6f3-8f4a-4c47-8e8b-69f0b1a7ec08"); 70e2aad8-6216-48cc-ab13-3439970a189a
                var response = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    orderCntAndList = JsonConvert.DeserializeObject<Orders>(content);

                    if (OrderType == 1)
                    {
                        OpenOrders.Clear();
                        foreach (var order in orderCntAndList.List)
                        {
                            OpenOrders.Add(order);
                        }
                        StartIdxOrd = MaxCntOrd;
                        MaxCntOrd = StartIdxOrd + 100;
                    }
                    if (OrderType == 2)
                    {
                        OpenReserv.Clear();
                        foreach (var order in orderCntAndList.List)
                        {
                            OpenReserv.Add(order);
                        }
                        StartIdxRes = MaxCntRes;
                        MaxCntRes = StartIdxRes + 100;
                    }
                    if (OrderType == 3)
                    {
                        OpenQuote.Clear();
                        foreach (var order in orderCntAndList.List)
                        {
                            OpenQuote.Add(order);
                        }
                        StartIdxQuote = MaxCntQuote;
                        MaxCntQuote = StartIdxQuote + 100;
                    }

                    OpenOrders_Original = new ObservableCollection<Order>(OpenOrders);
                    OpenReserv_Original = new ObservableCollection<Order>(OpenReserv);
                    OpenQuote_Original = new ObservableCollection<Order>(OpenQuote);

                }
            }
            catch (Exception ex)
            {

            }
        }
        internal void SearchForOrder(string text, int OrderType, bool isNewSearch)
        {
            try
            {
                bool resetData = false;
                if (string.IsNullOrEmpty(text))
                    resetData = true;

                IEnumerable<Order> filteredOrderData;

                if (OrderType == 1)
                {
                    OpenOrders.Clear();
                    if (resetData)
                    {
                        filteredOrderData = OpenOrders_Original.ToList();
                    }
                    else
                    {
                        filteredOrderData = OpenOrders_Original.Where(p => p.OrderNumberT.ToString().Contains(text)).ToList();
                    }
                    foreach (var item in filteredOrderData)
                    {
                        OpenOrders.Add(item);
                    }
                }
                if (OrderType == 2)
                {
                    OpenReserv.Clear();
                    if (resetData)
                    {
                        filteredOrderData = OpenReserv_Original.ToList();
                    }
                    else
                    {
                        filteredOrderData = OpenReserv_Original.Where(p => p.OrderNumberT.ToString().Contains(text)).ToList();
                    }
                    foreach (var item in filteredOrderData)
                    {
                        OpenReserv.Add(item);
                    }
                }
                if (OrderType == 3)
                {
                    OpenQuote.Clear();
                    if (resetData)
                    {
                        filteredOrderData = OpenQuote_Original.ToList();
                    }
                    else
                    {
                        filteredOrderData = OpenQuote_Original.Where(p => p.OrderNumberT.ToString().Contains(text)).ToList();
                    }
                    foreach (var item in filteredOrderData)
                    {
                        OpenQuote.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        internal void ClearSearchOrder(int OrderType)
        {
            try
            {
                if (OrderType == 1)
                {
                    OpenOrders.Clear();
                    foreach (var item in OpenOrders_Original)
                    {
                        OpenOrders.Add(item);
                    }
                }
                if (OrderType == 2)
                {
                    OpenReserv.Clear();
                    foreach (var item in OpenReserv_Original)
                    {
                        OpenReserv.Add(item);
                    }
                }
                if (OrderType == 3)
                {
                    OpenQuote.Clear();
                    foreach (var item in OpenQuote_Original)
                    {
                        OpenQuote.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public EditExistingOrdersViewModel()
        {
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
            OrdersEnabled = true;
        }

        HttpClient clientHttp;
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        List<Customer> custList = new List<Customer>();

        private int StoreID = 0;
        private string SearchText = "";
        private int StartIdxOrd = 0;
        private int StartIdxRes = 0;
        private int StartIdxQuote = 0;
        private int MaxCntOrd = 100;
        private int MaxCntRes = 100;
        private int MaxCntQuote = 100;

        public void GetOrdersInfo()
        {

            try
            {

            }
            catch (Exception ex)
            {
            }
        }
        private Order selectedOrder;
        public Order SelectedOrder
        {
            get { return selectedOrder; }
            set
            {
                if (selectedOrder != value)
                {
                    selectedOrder = value;
                }
            }
        }
    }
}
