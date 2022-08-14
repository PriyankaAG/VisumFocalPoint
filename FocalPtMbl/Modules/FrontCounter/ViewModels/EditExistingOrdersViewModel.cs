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
using System.Threading.Tasks;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{
    public class EditExistingOrdersViewModel : ThemeBaseViewModel
    {
        public ICommand SearchCommand { get; }
        public ICommand ClearCommand { get; }

        public string OrderToSearch { get; set; }
        public string ReservationToSearch { get; set; }
        public string QuotesToSearch { get; set; }

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

        internal async Task GetSearchedOrdersInfo(string text, int orderType, bool isNewSearch)
        {
            Indicator = true;
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
                var response = await ClientHTTP.PostAsync(uri, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    orderCntAndList = JsonConvert.DeserializeObject<Orders>(content);
                    //var ord = orderCntAndList.List.Where(x => x.Payments.Count > 0 && x.Totals.TotalDueAmt > 0);

                    var list = orderCntAndList.List.OrderByDescending(x => x.OrderNo).ToList();
                    if (OrderType == 1)
                    {
                        OpenOrders.Clear();
                        OpenOrders = new ObservableCollection<Order>(list);
                        StartIdxOrd = MaxCntOrd;
                        MaxCntOrd = StartIdxOrd + 100;
                    }
                    if (OrderType == 2)
                    {
                        OpenReserv.Clear();
                        OpenReserv = new ObservableCollection<Order>(list);
                        StartIdxRes = MaxCntRes;
                        MaxCntRes = StartIdxRes + 100;
                    }
                    if (OrderType == 3)
                    {
                        OpenQuote.Clear();
                        OpenQuote = new ObservableCollection<Order>(list);
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
            finally
            {
                Indicator = false;
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
            GetData();
            SearchCommand = new Command<string>((a) => Search(a));
            ClearCommand = new Command<string>((a) => Clear(a));
        }

        private async Task GetData()
        {
            try
            {
                Indicator = true;
                var t1 = Task.Run(() =>
                {
                    _ = GetSearchedOrdersInfo("", 1, true);
                });
                var t2 = Task.Run(() =>
                {
                    _ = GetSearchedOrdersInfo("", 2, true);
                });
                var t3 = Task.Run(() =>
                {
                    _ = GetSearchedOrdersInfo("", 3, true);
                });
            }
            finally
            {
                Indicator = false;
            }
        }

        private void Clear(string ordType)
        {
            if (Indicator)
                return;

            var orderType = Convert.ToInt32(ordType);
            Indicator = true;

            switch (orderType)
            {
                case 1:
                    GetSearchedOrdersInfo("", orderType, true);
                    Indicator = false;
                    break;
                case 2:
                    GetSearchedOrdersInfo("", orderType, true);
                    Indicator = false;
                    break;
                case 3:
                    GetSearchedOrdersInfo("", orderType, true);
                    Indicator = false;
                    break;
            }

        }
        private void Search(string ordType)
        {
            if (Indicator)
                return;

            var orderType = Convert.ToInt32(ordType);
            Indicator = true;
            switch (orderType)
            {
                case 1:
                    GetSearchedOrdersInfo(OrderToSearch, orderType, true);
                    Indicator = false;
                    break;
                case 2:
                    GetSearchedOrdersInfo(ReservationToSearch, orderType, true);
                    Indicator = false;
                    break;
                case 3:
                    GetSearchedOrdersInfo(QuotesToSearch, orderType, true);
                    Indicator = false;
                    break;
            }
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
