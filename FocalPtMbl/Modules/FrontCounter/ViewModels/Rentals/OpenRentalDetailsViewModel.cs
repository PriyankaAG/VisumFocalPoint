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
        private RentalAvailability availability = new RentalAvailability();
        public RentalAvailability Availability
        {
            get { return availability; }
            set
            {
                if (availability != value)
                {
                    availability = value;
                    OnPropertyChanged("Availability");
                }
            }
        }
        public DateTime SubGroupLastUpdated { get { return DateTime.Now; } }
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


        private DateTime? startTime = DateTime.Now;
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
        private DateTime? endTime = DateTime.Now;
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

        public void GetRentalAvailability()
        {
            try
            {
                Rental rental = CurrentRental;
                string ItemID = rental.RentalItem.ToString();
                var combinedStartDte = new DateTime();
                var combinedEndDte = new DateTime();
                if (StartTime != null)
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

                string StartDate = combinedStartDte.ToString();
                string EndDate = combinedEndDte.ToString();

                Indicator = true;
                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "RentalAvailability"));
                var stringContent = new StringContent(
                                          JsonConvert.SerializeObject(new
                                          {
                                              ItemID,
                                              StartDate,
                                              EndDate
                                          }),
                                          Encoding.UTF8,"application/json");
                var response = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    Availability = JsonConvert.DeserializeObject<RentalAvailability>(content);
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
