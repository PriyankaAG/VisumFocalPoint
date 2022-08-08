using FocalPoint.Data;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.FrontCounter.ViewModels.Rentals
{
    public class OpenRentalDetailsViewModel : ThemeBaseViewModel
    {
        IGeneralComponent generalComponent;
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

        internal async void GetRate()
        {
             await GetRentalRates();
        }

        private Rental currentRental = new Rental();
        public Rental CurrentRental
        {
            get { return currentRental; }
            set
            {
                if (currentRental != value)
                {
                    currentRental = value;
                    OnPropertyChanged("CurrentRental");
                }
            }
        }
        private RentalRate currentRentalRates = new RentalRate();
        public RentalRate CurrentRentalRates
        {
            get { return currentRentalRates; }
            set
            {
                if (currentRentalRates != value)
                {
                    currentRentalRates = value;
                    OnPropertyChanged("CurrentRentalRates");
                }
            }
        }

        public DateTime SubGroupLastUpdated { get { return DateTime.Now; } }


        HttpClient clientHttp;
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        public OpenRentalDetailsViewModel()
        {
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
            generalComponent = new GeneralComponent();
        }

        private async Task GetRentalRates()
        {
            try
            {
                Indicator = true;
                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "RentalRate/" + CurrentRental.RentalSubGroup));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                var response = await ClientHTTP.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    RentalRate rentalCntAndList = JsonConvert.DeserializeObject<RentalRate>(content);
                    CurrentRentalRates = rentalCntAndList;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    generalComponent.HandleTokenExpired();
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
