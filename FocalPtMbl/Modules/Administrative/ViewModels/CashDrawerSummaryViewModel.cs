using FocalPoint.Data;
using Visum.Services.Mobile.Entities;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace FocalPoint.Modules.Administrative.ViewModels
{
    public class CashDrawerSummaryViewModel : ThemeBaseViewModel
    {
        ObservableCollection<CashDrawerResult> recent;
        public ObservableCollection<CashDrawerResult> Recent
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
        private HttpClient PreparedClient()
        {
            HttpClientHandler handler = new HttpClientHandler();

            //not sure about this one, but I think it should work to allow all certificates:
            handler.ServerCertificateCustomValidationCallback += (sender, cert, chaun, ssPolicyError) =>
            {
                return true;
            };


            HttpClient client = new HttpClient(handler);

            return client;
        }

        internal async void GetCashDrawers()
        {
            try
            {
                Uri uriGetCashDrawer = new Uri(string.Format(DataManager.Settings.ApiUri + "CashDrawers/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                                                                                                            // uriGetCashDrawer = new Uri(string.Format(https://visumaaron.fpsdns.com:56883/Mobile/V1/CashDrawers));
                var responseDR = await ClientHTTP.GetAsync(uriGetCashDrawer);
                if (responseDR.IsSuccessStatusCode)
                {
                    var content = responseDR.Content.ReadAsStringAsync().Result;
                    var cashDrawersResult = JsonConvert.DeserializeObject<List<CashDrawer>>(content);
                    if (CurrentCashDrawers.Count > 0)
                        CurrentCashDrawers.Clear();
                    foreach (var foundCashDrawers in cashDrawersResult)
                        CurrentCashDrawers.Add(foundCashDrawers);
                }
            }
            catch (Exception ex)
            {

            }
        }


        private DateTime? selectedDate = new DateTime();
        public DateTime? SelectedDate
        {
            get { return selectedDate; }
            set
            {
                if (selectedDate != value)
                {
                    selectedDate = value;
                    OnPropertyChanged("SelectedDate");
                }
            }
        }
        private CashDrawer selectedCashDrawer;
        public CashDrawer SelectedCashDrawer
        {
            get { return selectedCashDrawer; }
            set
            {
                if (selectedCashDrawer != value)
                {
                    selectedCashDrawer = value;
                    OnPropertyChanged("SelectedCashDrawer");
                }
            }
        }
        private ObservableCollection<CashDrawer> currentCashDrawers;
        public ObservableCollection<CashDrawer> CurrentCashDrawers
        {
            get => this.currentCashDrawers;
            private set
            {
                this.currentCashDrawers = value;
                OnPropertyChanged(nameof(CurrentCashDrawers));
            }
        }


        internal string Validate()
        {
            if (string.IsNullOrEmpty(SelectedCashDrawer.CashDrawerName))
                return "Must Select Cash Drawer";
            if (SelectedDate == null || SelectedDate.HasValue == false)
                return "Must Select Date";
            return "";
        }

        internal async void GetCashDrawerSummary()
        {
            try
            {
                DateTime DateOf = DateTime.Today;
                string CashDrawer = "";
                CashDrawer cd = new CashDrawer();
                // if(SelectedDate.Value !=null)
                DateOf = SelectedDate.Value;
                int Store = 1;
                if (SelectedStore != null)
                    Store = SelectedStore.CmpNo;
                if (SelectedCashDrawer.CashDrawerName != "")
                    CashDrawer = SelectedCashDrawer.CashDrawerName;
                Uri uriCashDrawer = new Uri(string.Format(DataManager.Settings.ApiUri + "Reports/CashDrawerSummary/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                var stringContentDR = new StringContent(
                                          JsonConvert.SerializeObject(new { DateOf, CashDrawer }),
                                          Encoding.UTF8,
                                          "application/json");
                var responseDR = await ClientHTTP.PostAsync(uriCashDrawer, stringContentDR);
                if (responseDR.IsSuccessStatusCode)
                {
                    var content = responseDR.Content.ReadAsStringAsync().Result;
                    var cashDrawerResult = JsonConvert.DeserializeObject<CashDrawerResult>(content);
                    if (Recent.Count > 0)
                        Recent.Clear();
                    Recent.Add(cashDrawerResult);
                }
            }
            catch (Exception ex)
            {

            }
        }

        HttpClient clientHttp = new HttpClient();
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        internal void DateChanged(DateTime? SelectedDate)
        {
            selectedDate = SelectedDate;
        }
        public CashDrawerSummaryViewModel()
        {
            Recent = new ObservableCollection<CashDrawerResult>();
            CurrentStores = new ObservableCollection<Company>();
            SelectedStore = new Company();
            SelectedCashDrawer = new CashDrawer();
            CurrentCashDrawers = new ObservableCollection<CashDrawer>();
            SelectedDate = DateTime.Today;

            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
            //clientHttp = PreparedClient();
            //clientHttp.DefaultRequestHeaders.Add("Token", DataManager.Settings.UserToken);
            //ClientHTTP.DefaultRequestHeaders.Add("StoreNo", DataManager.Settings.HomeStore.ToString());
            //clientHttp.DefaultRequestHeaders.Add("TerminalNo", DataManager.Settings.Terminal.ToString());
            //GetCashDrawers();
        }
        private async void GetStores()
        {
            try
            {
                Indicator = true;
                Uri uriStores = new Uri(string.Format(DataManager.Settings.ApiUri + "LoginStores"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                var responseDR = await ClientHTTP.GetAsync(uriStores);
                if (responseDR.IsSuccessStatusCode)
                {
                    var content = responseDR.Content.ReadAsStringAsync().Result;
                    var Stores = JsonConvert.DeserializeObject<List<Company>>(content);
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
    }
}
