using FocalPoint.Data;
using FocalPtMbl.MainMenu.Data;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{
    public class SelectSerialsOnlyViewModel : ThemeBaseViewModel
    {
        //serials?
        private AvailabilityMerch selectedItem = new AvailabilityMerch();
        public AvailabilityMerch SelectedItem
        {
            get => this.selectedItem;
            set
            {
                this.selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        ObservableCollection<AvailabilityMerch> recent;

        HttpClient clientHttp;
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        internal void GetSerials(AvailabilityMerch selectedItem)
        {
            try
            {
                //update searchText
                List<string> merchSerials = null;
                string StoreNo = selectedItem.AvailCmp.ToString();
                string MerchNo = selectedItem.AvailItem.ToString();
                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "AvailabilityMerchandiseSerials/"+MerchNo+"/"+StoreNo));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));

                var response = ClientHTTP.GetAsync(uri).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    merchSerials = JsonConvert.DeserializeObject<List<string>>(content);
                    //StartIdx = customersCntAndList.TotalCnt;
                    if (recent == null)
                    {
                        SelectedSerialsFound = new ObservableCollection<string>(merchSerials);
                    }
                    else
                    {
                        Recent.Clear();
                        foreach (var merch in merchSerials)
                        {
                            SelectedSerialsFound.Add(merch);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public ObservableCollection<AvailabilityMerch> Recent
        {
            get => this.recent;
            private set
            {
                this.recent = value;
                OnPropertyChanged(nameof(Recent));
            }
        }
        private Visum.Services.Mobile.Entities.Order currentOrder = new Visum.Services.Mobile.Entities.Order();
        public Visum.Services.Mobile.Entities.Order CurrentOrder
        {
            get => this.currentOrder;
            set
            {
                this.currentOrder = value;
                OnPropertyChanged(nameof(CurrentOrder));
            }
        }
        private ObservableCollection<string> selectedSerialsFound = new ObservableCollection<string>();
        public ObservableCollection<string> SelectedSerialsFound
        {
            get => this.selectedSerialsFound;
            private set
            {
                this.selectedSerialsFound = value;
                OnPropertyChanged(nameof(SelectedSerialsFound));
            }

        }
        private ObservableCollection<string> selectedSerials = new ObservableCollection<string>();
        public ObservableCollection<string> SelectedSerials
        {
            get => this.selectedSerials;
            private set
            {
                this.selectedSerials = value;
                OnPropertyChanged(nameof(SelectedSerials));
            }

        }
        public SelectSerialsOnlyViewModel()
        {
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
        }
    }
    //private class SerialNumberDisplay()
    //{

    //}
}
