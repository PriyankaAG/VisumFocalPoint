using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.FrontCounter.ViewModels.Rentals
{
    public class OpenRentalDetailsViewModel : ThemeBaseViewModel
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

        internal void GetRate()
        {
            CurrentRentalRates = new RentalRate();
            //throw new NotImplementedException();
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




        HttpClient clientHttp;
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        public OpenRentalDetailsViewModel()
        {
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
        }

    }
}
