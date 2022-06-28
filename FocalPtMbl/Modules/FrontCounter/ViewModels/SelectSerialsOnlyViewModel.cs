using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{
    public class SelectSerialsOnlyViewModel : ThemeBaseViewModel
    {
        //serials?
        private AvailabilityMerch selectedItem = new AvailabilityMerch();

        public INewQuickRentalEntityComponent NewQuickRentalEntityComponent { get; set; }

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
        internal async Task GetSerials(AvailabilityMerch selectedItem)
        {
            try
            {
                //update searchText
                List<string> merchSerials = null;
                string StoreNo = selectedItem.AvailCmp.ToString();
                string MerchNo = selectedItem.AvailItem.ToString();
                merchSerials = await NewQuickRentalEntityComponent.AvailabilityMerchandiseSerials(MerchNo, StoreNo);
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
            catch (Exception ex)
            {
                //TODO: log error
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
            NewQuickRentalEntityComponent = new NewQuickRentalEntityComponent();
        }
    }
}