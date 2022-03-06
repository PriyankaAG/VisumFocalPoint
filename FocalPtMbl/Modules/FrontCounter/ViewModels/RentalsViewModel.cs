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
    public class RentalsViewModel : ThemeBaseViewModel
    {

        private DateTime? startDte = new DateTime();
        public DateTime? StartDte
        {
            get { return startDte; }
            set
            {
                if (startDte != value)
                {
                    startDte = value;
                    OnPropertyChanged(nameof(StartDte));
                }
            }
        }
        private DateTime? endDte = new DateTime();
        public DateTime? EndDte
        {
            get { return endDte; }
            set
            {
                if (endDte != value)
                {
                    endDte = value;
                    OnPropertyChanged(nameof(EndDte));
                }
            }
        }


        private DateTime? startTime = new DateTime();
        public DateTime? StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    OnPropertyChanged(nameof(StartTime));
                }
            }
        }
        private DateTime? endTime = new DateTime();
        public DateTime? EndTime
        {
            get { return endTime; }
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    OnPropertyChanged(nameof(EndTime));
                }
            }
        }


        internal void GetSearchedCustomersInfo(string text)
        {
            //update searchText
            SearchText = text;
            StartIdx = 0;
            Customers customersCntAndList = null;
            try { 
            //Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "Customers/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
            //var stringContent = new StringContent(
            //                          JsonConvert.SerializeObject(new { StoreID, SearchText, StartIdx, MaxCnt }),
            //                          Encoding.UTF8,
            //                          "application/json");
            ////ClientHTTP.DefaultRequestHeaders.Add("Token", "987919a1-b105-4c16-99d8-9c8ec2b81dcf");//"3d2ad6f3-8f4a-4c47-8e8b-69f0b1a7ec08"); 70e2aad8-6216-48cc-ab13-3439970a189a
            //var response = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
            //if (response.IsSuccessStatusCode)
            //{
            //    string content = response.Content.ReadAsStringAsync().Result;
            //    customersCntAndList = JsonConvert.DeserializeObject<Customers>(content);
            //    StartIdx = customersCntAndList.TotalCnt;
            //    if (recent == null)
            //    {
            //        Recent = new ObservableCollection<Customer>(customersCntAndList.List);
            //    }
            //    else
            //    {
            //        Recent.Clear();
            //        foreach (var customer in customersCntAndList.List)
            //        {
            //            Recent.Add(customer);
            //        }
            //    }
            //}
            }
            catch (Exception ex)
            {

            }
        }

        internal void DateChanged(DateTime? startDate)
        {
            //throw new NotImplementedException();
        }


        internal void TimeChanged(object startTime)
        {
           // throw new NotImplementedException();
        }

        public RentalsViewModel()
        {
            Recent = new ObservableCollection<RentalAvailability>();
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
            CurrentStores = new ObservableCollection<Company>();
            GetCurrentStores();
            // GetRentVal();
        }

        private void GetCurrentStores()
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

        internal void StoreChanged(object selectedStore)
        {
           // throw new NotImplementedException();
        }

        HttpClient clientHttp = new HttpClient();

        internal void SearchInChanged(object selectedSearchIn)
        {
          //  throw new NotImplementedException();
        }

        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        ObservableCollection<RentalAvailability> recent;
        public ObservableCollection<RentalAvailability> Recent
        {
            get => this.recent;
            private set
            {
                this.recent = value;
                OnPropertyChanged(nameof(Recent));
            }
        }


        internal ObservableCollection<RentalAvailability> SearchButtonClicked()
        {
            // throw new NotImplementedException();
            if (SelectedStore != null && SearchTextEdit != null && SelectedSearchIn != null && StartDte != null && EndDte != null)
            {
                short SearchType = 0;
                string Search = SearchTextEdit;
                if (SelectedSearchIn == "Rental File")
                    SearchType = 1;
                if (SelectedSearchIn == "Rate Table")
                    SearchType = 2;
                var combinedStartDte = new DateTime();
                var combinedEndDte = new DateTime();
                if (StartTime != null )
                {
                     combinedStartDte = new DateTime(StartDte.Value.Year, StartDte.Value.Month, StartDte.Value.Day, StartTime.Value.Hour, StartTime.Value.Minute, 0);
                    
                }
                else
                {
                     combinedStartDte = new DateTime(StartDte.Value.Year, StartDte.Value.Month, StartDte.Value.Day, 0, 0, 0);
                }
                if (EndTime != null)
                {
                     combinedEndDte = new DateTime(EndDte.Value.Year, EndDte.Value.Month, EndDte.Value.Day, EndTime.Value.Hour, EndTime.Value.Minute, 0);
                }
                else
                {
                     combinedEndDte = new DateTime(EndDte.Value.Year, EndDte.Value.Month, EndDte.Value.Day, 0, 0, 0);
                }

                int ShowStoreID = SelectedStore.CmpNo;
                string StartDate = combinedStartDte.ToString();
                string EndDate = combinedEndDte.ToString();
                try
                {
                    Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "RentalsAvailability/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                    var stringContent = new StringContent(
                                              JsonConvert.SerializeObject(new { SearchType, Search, ShowStoreID, StartDate, EndDate }),
                                              Encoding.UTF8,
                                              "application/json");
                    var response = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        string content = response.Content.ReadAsStringAsync().Result;
                        var rentalAvailabilities = JsonConvert.DeserializeObject<List<RentalAvailability>>(content);
                        if (recent == null)
                        {
                            Recent = new ObservableCollection<RentalAvailability>(rentalAvailabilities);
                        }
                        else
                        {
                            Recent.Clear();
                            foreach (var rental in rentalAvailabilities)
                            {
                                Recent.Add(rental);
                            }
                        }
                    }
                    return Recent;
                }
                catch (Exception ex)
                {
                    return Recent;
                }
            }
            else
            {
                return null;
            }
        }

        List<Customer> custList = new List<Customer>();

        private int StoreID = 0;
        private string SearchText = "";
        private int StartIdx = 0;
        private int MaxCnt = 100;

        public Customers GetCustomersInfo()
        {
            Customers customersCntAndList = null;
            //Customer customersist = null;
            try
            {
            //    Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "Customers/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
            //    var stringContent = new StringContent(
            //                              JsonConvert.SerializeObject(new { StoreID, SearchText, StartIdx, MaxCnt }),
            //                              Encoding.UTF8,
            //                              "application/json");
            //    // var productValue = new ProductInfoHeaderValue("FocalPoint Mobile\3.0.0"); //"FocalPoint Mobile\\3.0.0"
            //    // var commentValue = new ProductInfoHeaderValue("(+http://www.example.com/ScraperBot.html)");
            //    // ClientHTTP.DefaultRequestHeaders.UserAgent.Add(productValue); 05fe29ff-6640-487b-9331-6b5759851bca

            //    //"3d2ad6f3-8f4a-4c47-8e8b-69f0b1a7ec08"); 70e2aad8-6216-48cc-ab13-3439970a189a
            //    var response = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
            //    if (response.IsSuccessStatusCode)
            //    {
            //        string content = response.Content.ReadAsStringAsync().Result;
            //        // var asdf  = JsonConvert.DeserializeObject(content);
            //        customersCntAndList = JsonConvert.DeserializeObject<Customers>(content);
            //        StartIdx = customersCntAndList.TotalCnt;
            //        if (recent == null)
            //        {
            //            Recent = new ObservableCollection<Customer>(customersCntAndList.List);
            //        }
            //        else
            //        {
            //            foreach (var customer in customersCntAndList.List)
            //            {
            //                Recent.Add(customer);
            //            }
            //        }

            //        //var custList = JsonConvert.DeserializeObject<List<FocalPoint.Data.DataLayer.Customer>(content);
            //        // Recent = (ObservableCollection<FocalPoint.Data.DataLayer.Customer>)CustList;
            //    }
            return customersCntAndList;
            }
            catch (Exception ex)
            { return customersCntAndList; }
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

        public string SearchTextEdit { get; internal set; }
        public string SelectedSearchIn { get; internal set; }

    }
}
