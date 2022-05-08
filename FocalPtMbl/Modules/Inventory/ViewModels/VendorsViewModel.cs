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

namespace FocalPoint.Modules.Inventory.ViewModels
{
    public class VendorsViewModel : ThemeBaseViewModel
    {
        ObservableCollection<Vendor> recent;
        public ObservableCollection<Vendor> Recent
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

        public VendorsViewModel()
        {
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
            //var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes("c6760347-c341-47c6-9d90-8171615edc92"));

            //clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
        }

        HttpClient clientHttp;
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        //List<Vendor> vendList = new List<Data.DataLayer.Vendor>();
        private int StoreID = 0;
        private string SearchText = "";
        private int StartIdx = 0;
        private int MaxCnt = 100;
        public Vendors GetVendorsInfo()
        {
            Vendors vendorsCntAndList = null;
            try
            {

                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "Vendors/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                var stringContent = new StringContent(
                                          JsonConvert.SerializeObject(new { StoreID, SearchText, StartIdx, MaxCnt }),
                                          Encoding.UTF8,
                                          "application/json");

               // ClientHTTP.DefaultRequestHeaders.Add("Token", "581543bd-ac48-414b-a356-643b2403eba3");//"3d2ad6f3-8f4a-4c47-8e8b-69f0b1a7ec08");
                var response = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    vendorsCntAndList = JsonConvert.DeserializeObject<Vendors>(content);
                    StartIdx = vendorsCntAndList.TotalCnt;
                    if (recent == null)
                    {
                        Recent = new ObservableCollection<Vendor>(vendorsCntAndList.List);
                    }
                    else
                    {
                        foreach (var vendor in vendorsCntAndList.List)
                        {
                            Recent.Add(vendor);
                        }
                    }

                }
                return vendorsCntAndList;
            }
            catch (Exception ex)
            { return vendorsCntAndList; }
        }

        internal void GetSearchedVendorsInfo(string text)
        {
            Recent.Clear();
            SearchText = text;
            StartIdx = 0; 
            Vendors vendorsCntAndList = null;
            try { 
            Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "Vendors/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
            var stringContent = new StringContent(
                                      JsonConvert.SerializeObject(new { StoreID, SearchText, StartIdx, MaxCnt }),
                                      Encoding.UTF8,
                                      "application/json");

            var response = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                vendorsCntAndList = JsonConvert.DeserializeObject<Vendors>(content);
                StartIdx = vendorsCntAndList.TotalCnt;
                if (recent == null)
                {
                    Recent = new ObservableCollection<Vendor>(vendorsCntAndList.List);
                }
                else
                {
                    foreach (var vendor in vendorsCntAndList.List)
                    {
                        Recent.Add(vendor);
                    }
                }

            }
            }
            catch (Exception ex)
            {

            }
        }

        void ExecuteLoadMoreCommand()
        {
        }
        private Vendor selectedVendor;
        public Vendor SelectedVendor
        {
            get { return selectedVendor; }
            set
            {
                if (selectedVendor != value)
                {
                    selectedVendor = value;
                }
            }
        }
    }
}
