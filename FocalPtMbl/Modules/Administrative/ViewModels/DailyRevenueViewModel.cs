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

namespace FocalPoint.Modules.Administrative.ViewModels
{
   public class DailyRevenueViewModel : ThemeBaseViewModel
    {
        ObservableCollection<DailyRevenue> recent;
        public ObservableCollection<DailyRevenue> Recent
        {
            get => this.recent;
            private set
            {
                this.recent = value;
                OnPropertyChanged(nameof(Recent));
            }
        }
        public ObservableCollection<Company> currentStores;
        private Company selectedStore;
        public Company SelectedStore
        {
            get => this.selectedStore;
            set
            {
                this.selectedStore = value;
                OnPropertyChanged(nameof(SelectedStore));
            }
        }
        public ObservableCollection<Company> CurrentStores
        {
            get => this.currentStores;
            private set
            {
                this.currentStores = value;
                OnPropertyChanged(nameof(CurrentStores));
            }
        }

        public ObservableCollection<PostCode> currentPostCodes;


        private PostCode selectedPostCode;
        public PostCode SelectedPostCode
        {
            get => this.selectedPostCode;
            set
            {
                this.selectedPostCode = value;
                OnPropertyChanged(nameof(SelectedPostCode));
            }
        }
        public ObservableCollection<PostCode> CurrentPostCodes
        {
            get => this.currentPostCodes;
            private set
            {
                this.currentPostCodes = value;
                OnPropertyChanged(nameof(CurrentPostCodes));
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

        internal void DateChanged(DateTime? SelectedDate)
        {
            selectedDate = SelectedDate;
          }

    private DailyRevenue dailyRevDL = new DailyRevenue();
        public DailyRevenue DailyRevDL
        {
            get { return dailyRevDL; }
            set
            {
                if (dailyRevDL != value)
                {
                    dailyRevDL = value;
                    OnPropertyChanged("DailyRevDL");
                }
            }
        }
        public string Revenue
        {
            get { return dailyRevDL.Revenue.ToString(); }
        }
        public string Receipts
        {
            get { return dailyRevDL.Receipts.ToString(); }
        }
        public string Inventory
        {
            get { return dailyRevDL.Inventory.ToString(); }
        }
        public string COGS
        {
            get { return dailyRevDL.COGS.ToString(); }
        }
        public string Deposits
        {
            get { return dailyRevDL.Deposits.ToString(); }
        }
        public string Repairs
        {
            get { return dailyRevDL.Repairs.ToString(); }
        }
        public string Warranty
        {
            get { return dailyRevDL.Warranty.ToString(); }
        }
        public string Labor
        {
            get { return dailyRevDL.Labor.ToString(); }
        }

        public DailyRevenueViewModel()
        {
            Recent = new ObservableCollection<DailyRevenue>();
            CurrentPostCodes = new ObservableCollection<PostCode>();
            CurrentStores = new ObservableCollection<Company>();
            SelectedStore = new Company();
            SelectedPostCode = new PostCode();
            SelectedDate = DateTime.Today;

            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();

            // ClientHTTP.DefaultRequestHeaders.Add("Token", "581543bd-ac48-414b-a356-643b2403eba3");
            //add store
            // ClientHTTP.DefaultRequestHeaders.Add("StoreNo", "1");
            //ClientHTTP.DefaultRequestHeaders.Add("TerminalNo", "3");
            GetLoginStores();
            GetPostCodes();
            //GetDailyRev();
        }

            HttpClient clientHttp = null;
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        //List<Vendor> vendList = new List<Data.DataLayer.Vendor>();
        public void GetLoginStores()
        {
            try
            {
                Uri uriStores = new Uri(string.Format(DataManager.Settings.ApiUri + "LoginStores"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                var responseDR = ClientHTTP.GetAsync(uriStores).GetAwaiter().GetResult();
                if (responseDR.IsSuccessStatusCode)
                {
                    var content = responseDR.Content.ReadAsStringAsync().Result;
                    var Stores = JsonConvert.DeserializeObject<List<Company>>(content);
                    foreach (var store in Stores)
                        CurrentStores.Add(store);
                }
            }catch(Exception ex)
            {

            }
        }
        private DateTime? selectedDate = new DateTime();
        public DateTime? SelectedDate
        {
            get {return selectedDate; }
            set
            {
                if (selectedDate != value)
                {
                selectedDate = value;
                    OnPropertyChanged("SelectedDate");
                }
            }
        }
        private void GetPostCodes()
        {
            try
            {
                Uri uriPostCodes = new Uri(string.Format(DataManager.Settings.ApiUri + "PostCodes"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                                                                                                     //var stringContentDR = new StringContent(
                                                                                                     //                          JsonConvert.SerializeObject(new { RevDate, Store, PostCode }),
                                                                                                     //                          Encoding.UTF8,
                                                                                                     //                          "application/json");
                var responseDR = ClientHTTP.GetAsync(uriPostCodes).GetAwaiter().GetResult();
                if (responseDR.IsSuccessStatusCode)
                {
                    var content = responseDR.Content.ReadAsStringAsync().Result;
                    var PostCodes = JsonConvert.DeserializeObject<List<PostCode>>(content);
                    foreach (var post in PostCodes)
                        CurrentPostCodes.Add(post);
                }
            }catch(Exception ex)
            {

            }
        }
        private void GetStores()
        {
            Uri uriStores = new Uri(string.Format(DataManager.Settings.ApiUri + "LoginStores"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
            var responseDR = ClientHTTP.GetAsync(uriStores).GetAwaiter().GetResult();
            if (responseDR.IsSuccessStatusCode)
            {
                var content = responseDR.Content.ReadAsStringAsync().Result;
                var Stores = JsonConvert.DeserializeObject<List<Company>>(content);
            }
        }
        internal string Validate()
        {
            if (SelectedStore == null || SelectedStore.CmpNo == 0)
                return "Must Select Store";
            if (SelectedPostCode == null || SelectedPostCode.No == 0)
                return "Must Select Post";
            if (SelectedDate == null || SelectedDate.HasValue == false)
                return "Must Select Date";
            return "";
        }

        public void GetDailyRev()
        {
            string RevDate = DateTime.Today.ToString();
            if (selectedDate.HasValue)
                RevDate = selectedDate.Value.ToString();
            int Store = 1;
            int PostCode = 0;
            if (SelectedPostCode != null)
                PostCode = SelectedPostCode.No;
            if (SelectedStore != null)
                Store = SelectedStore.CmpNo;
            Uri uriDailyRev = new Uri(string.Format(DataManager.Settings.ApiUri + "Reports/DailyRevenue//"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
            var stringContentDR = new StringContent(
                                      JsonConvert.SerializeObject(new { RevDate, Store, PostCode }),
                                      Encoding.UTF8,
                                      "application/json");
            var responseDR = ClientHTTP.PostAsync(uriDailyRev, stringContentDR).GetAwaiter().GetResult();
            if (responseDR.IsSuccessStatusCode)
            {
                var content = responseDR.Content.ReadAsStringAsync().Result;
                var DailyRev = JsonConvert.DeserializeObject<DailyRevenue>(content);
                if (Recent.Count > 0)
                    Recent.Clear();
                Recent.Add(DailyRev);
            }
        }

        void ExecuteLoadMoreCommand()
        {
        }
    }
}

