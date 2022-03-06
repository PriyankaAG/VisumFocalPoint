using FocalPoint.Data;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.FrontCounter.ViewModels.Rentals
{
    public class OpenRentalsViewModel : ThemeBaseViewModel
    {
        ObservableCollection<Rental> recent;
        public ObservableCollection<Rental> Recent
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



        HttpClient clientHttp;
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        public OpenRentalsViewModel()
        {
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
        }
        private string SearchText = "";
        private int StartIdx = 0;
        private int MaxCnt = 100;
        internal void GetRentals()
        {
            Visum.Services.Mobile.Entities.Rentals rentalCntAndList = null;
            try
            {

                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "Rentals/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                var stringContent = new StringContent(
                                          JsonConvert.SerializeObject(new { SearchText, StartIdx, MaxCnt }),
                                          Encoding.UTF8,
                                          "application/json");


                var response = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    rentalCntAndList = JsonConvert.DeserializeObject<Visum.Services.Mobile.Entities.Rentals>(content);
                    StartIdx = rentalCntAndList.TotalCnt;
                    if (recent == null)
                    {
                        Recent = new ObservableCollection<Rental>(rentalCntAndList.List);
                    }
                    else
                    {
                        foreach (var rental in rentalCntAndList.List)
                        {
                            Recent.Add(rental);
                        }
                    }

                }
               
            }
            catch (Exception ex)
            { 
            }

        }
    }
}
