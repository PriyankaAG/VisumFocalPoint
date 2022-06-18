using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using FocalPtMbl.MainMenu.ViewModels;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using FocalPoint.Modules.FrontCounter.Views.NewRentals;
using FocalPtMbl.MainMenu.ViewModels;
using FocalPtMbl.MainMenu.ViewModels.Services;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.FrontCounter.ViewModels.NewRental
{
    public class NewQuickRentalMainPageViewModel : ThemeBaseViewModel
    {
        public INewQuickRentalEntityComponent NewQuickRentalEntityComponent { get; set; }

        OrderUpdate _orderUpdate;
        public OrderUpdate OrderUpdate
        {
            get
            {
                return _orderUpdate;
            }
            set
            {
                _orderUpdate = value;
            }
        }

        Order _currentOrder;
        public Order CurrentOrder
        {
            get
            {
                return _currentOrder;
            }
            set
            {
                _currentOrder = value;
            }
        }

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
            NewQuickRentalEntityComponent = new NewQuickRentalEntityComponent();
        }

        internal async Task<List<string>> CreateNewOrder()
        {
            OrderUpdate = await NewQuickRentalEntityComponent.GetNewOrderCreationDetail();
            if (OrderUpdate != null && OrderUpdate.Order != null)
            {
                CurrentOrder = OrderUpdate.Order;

                //SelectedCustomerNameBox = CurrentOrder.Customer.CustomerName + " " + Regex.Replace(CurrentOrder.Customer.CustomerPhone, @"(\d{3})(\d{3})(\d{4})", "($1)$2-$3") + Environment.NewLine + "Type: " + displayCustType + " " + CurrentOrder.Customer.CustomerCity + ", " + CurrentOrder.Customer.CustomerState + " " + CurrentOrder.Customer.CustomerZip + " ";

                SelectedCustomer = new Customer();
                SelectedCustomer = CurrentOrder.Customer;
                RefreshAllProperties();
                if (OrderUpdate.Notifications.Count > 0)
                    return OrderUpdate.Notifications;
            }
            return null;
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
