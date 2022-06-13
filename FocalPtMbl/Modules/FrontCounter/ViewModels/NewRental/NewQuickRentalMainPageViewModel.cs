using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using FocalPoint.Modules.FrontCounter.Views.NewRentals;
using FocalPtMbl.MainMenu.ViewModels;
using FocalPtMbl.MainMenu.ViewModels.Services;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.FrontCounter.ViewModels.NewRental
{
    public class NewQuickRentalMainPageViewModel : ThemeBaseViewModel
    {
        Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get
            {
                return _selectedCustomer;
            }
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
                OnPropertyChanged(nameof(IsCustomerSelected));
            }
        }

        string _selectedItem;
        public string SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public string CustomerPhoneFormatted
        {
            get
            {
                var strToReturn = "";
                if (SelectedCustomer == null)
                    strToReturn = "Phone: N/A";
                else
                    strToReturn = Regex.Replace(SelectedCustomer?.CustomerPhone, @"(\d{3})(\d{3})(\d{4})", "($1)$2-$3");

                return strToReturn;
            }
        }
        public string CustomerAddressFormatted
        {
            get
            {
                return SelectedCustomer?.CustomerCity + ", " + SelectedCustomer?.CustomerState + " " + SelectedCustomer?.CustomerZip;
            }
        }
        public string CustomerTypeFormatted
        {
            get
            {
                var displayCustType = SelectedCustomer?.CustomerType;
                if (SelectedCustomer?.CustomerType == "c")
                    displayCustType = "Credit";
                if (SelectedCustomer?.CustomerType == "C")
                    displayCustType = "Credit";

                return displayCustType;
            }
        }

        public bool IsCustomerSelected
        {
            get
            {
                return SelectedCustomer != null;
            }
        }
        public bool IsCustomerNotSelected
        {
            get
            {
                return SelectedCustomer == null;
            }
        }
        public NewQuickRentalMainPageViewModel()
        {
            SelectedCustomer = null;
        }

        public void RefreshAllProperties()
        {
            OnPropertyChanged(nameof(CustomerPhoneFormatted));
            OnPropertyChanged(nameof(CustomerAddressFormatted));
            OnPropertyChanged(nameof(CustomerTypeFormatted));
            OnPropertyChanged(nameof(IsCustomerSelected));
            OnPropertyChanged(nameof(IsCustomerNotSelected));
        }
    }
}
