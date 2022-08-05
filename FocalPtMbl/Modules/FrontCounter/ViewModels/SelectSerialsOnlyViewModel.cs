﻿using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        internal async Task GetSerials(int AvailCmp, int AvailItem)
        {
            try
            {
                //update searchText
                List<MerchandiseSerial> merchSerials = null;
                string StoreNo = selectedItem.AvailCmp.ToString();
                string MerchNo = selectedItem.AvailItem.ToString();
                merchSerials = await NewQuickRentalEntityComponent.AvailabilityMerchandiseSerials(MerchNo, StoreNo);
                //StartIdx = customersCntAndList.TotalCnt;
                if (recent == null)
                {
                    SelectedSerialsFound = new ObservableCollection<MerchandiseSerial>(merchSerials);
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
        private ObservableCollection<MerchandiseSerial> selectedSerialsFound = new ObservableCollection<MerchandiseSerial>();
        public ObservableCollection<MerchandiseSerial> SelectedSerialsFound
        {
            get => this.selectedSerialsFound;
            private set
            {
                this.selectedSerialsFound = value;
                OnPropertyChanged(nameof(SelectedSerialsFound));
            }

        }

        public bool AddToSelectedSerial(MerchandiseSerial value)
        {
            if (!SelectedSerials.Any(x => x.MerchSerSerial == value.MerchSerSerial))
            {
                SelectedSerials.Add(value);
                return true;
            }
            return false;
        }

        private ObservableCollection<MerchandiseSerial> selectedSerials = new ObservableCollection<MerchandiseSerial>();
        public ObservableCollection<MerchandiseSerial> SelectedSerials
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