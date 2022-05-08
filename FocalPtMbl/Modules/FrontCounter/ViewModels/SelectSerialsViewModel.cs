using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{
    public class SelectSerialsViewModel : ThemeBaseViewModel
    {
        //serials?
        ObservableCollection<AvailabilityMerch> recent;
        public ObservableCollection<AvailabilityMerch> Recent
        {
            get => this.recent;
            private set
            {
                this.recent = value;
                OnPropertyChanged(nameof(Recent));
            }
        }
        private Order currentOrder = new Order();
        public Order CurrentOrder
        {
            get => this.currentOrder;
            set
            {
                this.currentOrder = value;
                OnPropertyChanged(nameof(Recent));
            }
        }
        private List<string> selectedSerials = new List<string>();
        public List<string> SelectedSerials
        {
            get => this.selectedSerials;
            private set
            {
                this.selectedSerials = value;
                OnPropertyChanged(nameof(Recent));
            }

        }
        private decimal quantOut = 0;
        public decimal QuantOut
        {
            get => this.quantOut;
            set
            {
                this.quantOut = value;
                OnPropertyChanged(nameof(QuantOut));
            }
        }
        private decimal discount = 0;
        public decimal Discount
        {
            get => this.discount;
            set
            {
                this.discount = value;
                OnPropertyChanged(nameof(Discount));
            }
        }
        private decimal overridePricing = 0;
        public decimal OverridePricing
        {
            get => this.overridePricing;
            set
            {
                this.overridePricing = value;
                OnPropertyChanged(nameof(OverridePricing));
            }
        }
        public SelectSerialsViewModel()
        {

        }
    }
}
