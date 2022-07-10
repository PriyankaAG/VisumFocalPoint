using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using FocalPtMbl.MainMenu.ViewModels;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using FocalPoint.Modules.FrontCounter.Views.NewRentals;
using FocalPtMbl.MainMenu.ViewModels.Services;
using Visum.Services.Mobile.Entities;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using FocalPoint.Utils;

namespace FocalPoint.Modules.FrontCounter.ViewModels.NewRental
{
    public class NewQuickRentalMainPageViewModel : ThemeBaseViewModel
    {
        public INewQuickRentalEntityComponent NewQuickRentalEntityComponent { get; set; }
        public OrderUpdate CurrentOrderUpdate { get; set; }
        bool isPageLoading;
        public bool IsPageLoading
        {
            get
            {
                return isPageLoading;
            }
            set
            {
                isPageLoading = value;
                OnPropertyChanged("IsPageLoading");
            }
        }
        public ICommand ExemptIDEntered => new Command(() => onExemptIDEntered());
        public async void onExemptIDEntered()
        {
            var orderRefresh = await UpdateDateValues();
        }

        public DateTime SelectedStartDateTime { get; set; }
        public DateTime SelectedEndDateTime { get; set; }

        public string SelectedStartString
        {
            get
            {
                return SelectedStartDateTime.ToString("g");
            }
        }
        public string SelectedEndString
        {
            get
            {
                return SelectedEndDateTime.ToString("g");
            }
        }

        private TimeSpan startTime;
        private TimeSpan endTime;
        public TimeSpan StartTime
        {
            get
            {
                return SelectedStartDateTime.TimeOfDay;
            }
            set
            {
                SelectedStartDateTime = new DateTime(SelectedStartDateTime.Year, SelectedStartDateTime.Month, SelectedStartDateTime.Day, value.Hours, value.Minutes, 0);

                RefreshDateTimeProperties();
            }
        }
        public TimeSpan EndTime
        {
            get
            {
                return SelectedEndDateTime.TimeOfDay;
            }
            set
            {
                SelectedEndDateTime = new DateTime(SelectedEndDateTime.Year, SelectedEndDateTime.Month, SelectedEndDateTime.Day, value.Hours, value.Minutes, 0);

                RefreshDateTimeProperties();
            }
        }

        public DateTime StartDate
        {
            get
            {
                return SelectedStartDateTime.Date;
            }
            set
            {
                SelectedStartDateTime = new DateTime(value.Year, value.Month, value.Day, SelectedStartDateTime.Hour, SelectedStartDateTime.Minute, 0);

                RefreshDateTimeProperties();
            }
        }
        public DateTime EndDate
        {
            get
            {
                return SelectedEndDateTime.Date;
            }
            set
            {
                SelectedEndDateTime = new DateTime(value.Year, value.Month, value.Day, SelectedEndDateTime.Hour, SelectedEndDateTime.Minute, 0);

                RefreshDateTimeProperties();
            }
        }

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
        OrderSettings _theOrderSettings;
        public OrderSettings TheOrderSettings
        {
            get
            {
                return _theOrderSettings;
            }
            set
            {
                _theOrderSettings = value;
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
                if (_currentOrder != null)
                {
                    BalanceDue = _currentOrder.OrderAmount - _currentOrder.OrderPaid;
                    OnPropertyChanged("OrderAmount");
                    OnPropertyChanged("IsSaveEnabled");
                }
            }
        }
        public bool IsSaveEnabled
        {
            get
            {
                return CurrentOrder != null && CurrentOrder.Customer != null;
            }
        }
        Decimal? _balanceDue;
        public Decimal? BalanceDue
        {
            get
            {
                return _balanceDue;
            }
            set
            {
                _balanceDue = value;
                OnPropertyChanged("BalanceDue");
            }
        }
        public Decimal? OrderAmount
        {
            get
            {
                return CurrentOrder?.OrderAmount;
            }
            set
            {
                CurrentOrder.OrderAmount = value.HasValue ? value.Value : 0;
                OnPropertyChanged("OrderAmount");
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
                OnPropertyChanged(nameof(SelectedCustomerEmail));
                OnPropertyChanged(nameof(IsCustomerSelected));
                OnPropertyChanged("IsSaveEnabled");
            }
        }
        Customer _selectedCustomerEmail;
        public string SelectedCustomerEmail
        {
            get
            {
                if (SelectedCustomer == null) return "Email N/A";

                return SelectedCustomer.CustomerEmail.HasData() ? SelectedCustomer.CustomerEmail : "Email N/A";
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
                if (SelectedCustomer == null || SelectedCustomer.CustomerPhone == null)
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

        public string[] LengthList { get; set; }
        private string _selectedLength;
        public string SelectedLength
        {
            get
            {
                return _selectedLength;
            }
            set
            {
                _selectedLength = value;

                OnPropertyChanged(nameof(SelectedLength));
            }
        }
        private string _exemptID;
        public string ExemptID
        {
            get
            {
                return _exemptID;
            }
            set
            {
                _exemptID = value;

                OnPropertyChanged(nameof(ExemptID));
            }
        }


        public string[] TaxList { get; set; }
        private string _selectedTax;
        public string SelectedTax
        {
            get
            {
                return _selectedTax;
            }
            set
            {
                _selectedTax = value;

                OnPropertyChanged(nameof(SelectedTax));
            }
        }

        private ObservableCollection<OrderDtl> recent;
        public ObservableCollection<OrderDtl> Recent
        {
            get { return recent; }
            set { recent = value; }
        }

        public NewQuickRentalMainPageViewModel() : base("Quick Rental")
        {
            SelectedCustomer = null;
            NewQuickRentalEntityComponent = new NewQuickRentalEntityComponent();


            RefreshDateTimeProperties();
            Recent = new ObservableCollection<OrderDtl>();

            MessagingCenter.Subscribe<AddDetailMerchView, OrderUpdate>(this, "UpdateOrder", (sender, arg) =>
            {
                //update order
                //UpdateCust(arg.Order.Customer);
                CurrentOrder = arg.Order;
                //Sel.Clear();
                //selCust.Add(arg.Order.Customer);
                Recent.Clear();
                foreach (var item in arg.Order.OrderDtls)
                    Recent.Add(item);

            });
            MessagingCenter.Subscribe<AddDetailRentalView, OrderUpdate>(this, "UpdateOrder", (sender, arg) =>
            {
                //update order
                //UpdateCust(arg.Order.Customer);
                CurrentOrder = arg.Order;
                //selCust.Clear();
                //selCust.Add(arg.Order.Customer);
                Recent.Clear();
                foreach (var item in arg.Order.OrderDtls)
                    Recent.Add(item);

            });
        }
        public void ReloadRecents(OrderDtl ordDtl)
        {
            if (Recent.Contains(ordDtl))
            {
                var index = Recent.IndexOf(ordDtl);
                Recent.Remove(ordDtl);
                Recent.Insert(index, ordDtl);
            }

               OnPropertyChanged(nameof(Recent));
        }

        public void RefreshDateTimeProperties()
        {
            OnPropertyChanged("StartTime");
            OnPropertyChanged("EndTime");
            OnPropertyChanged("StartDate");
            OnPropertyChanged("EndDate");

            OnPropertyChanged("SelectedStartString");
            OnPropertyChanged("SelectedEndString");
        }

        public void PopulateMasters()
        {
            List<string> itemHolders = new List<string>();
            //Length
            itemHolders.Add("Select Length");
            if (TheOrderSettings != null)
            {
                foreach (var item in TheOrderSettings?.Lengths)
                {
                    if (string.IsNullOrEmpty(item.Display)) continue;

                    itemHolders.Add(item.Display);
                }
            }
            LengthList = itemHolders.ToArray();

            //Tax
            itemHolders.Clear();
            itemHolders.Add("Select Tax");
            if (TheOrderSettings != null)
            {
                foreach (var item in TheOrderSettings?.TaxCodes)
                {
                    if (string.IsNullOrEmpty(item.Display)) continue;

                    itemHolders.Add(item.Display);
                }
                TaxList = itemHolders.ToArray();
            }

            OnPropertyChanged(nameof(LengthList));
            OnPropertyChanged(nameof(TaxList));

        }

        public void SetDefaultValues()
        {
            if (TheOrderSettings != null)
            {
                var selLen = TheOrderSettings?.Lengths.Find(p => p.Value == TheOrderSettings.Defaults.OrderLength);
                var selLenStr = selLen?.Display;
                SelectedLength = selLenStr;

                var selTax = TheOrderSettings?.TaxCodes.Find(p => p.Value == TheOrderSettings.Defaults.OrderTaxCode);
                var selTaxDisplay = selTax?.Display;
                SelectedTax = selTaxDisplay;

                SelectedStartDateTime = TheOrderSettings.Defaults.OrderODte;
                SelectedEndDateTime = TheOrderSettings.Defaults.OrderDDte;

            }

            RefreshDateTimeProperties();
            OnPropertyChanged(nameof(SelectedTax));
            OnPropertyChanged(nameof(SelectedLength));
        }

        public void RefreshAllProperties()
        {
            OnPropertyChanged(nameof(CustomerPhoneFormatted));
            OnPropertyChanged(nameof(CustomerAddressFormatted));
            OnPropertyChanged(nameof(CustomerTypeFormatted));
            OnPropertyChanged(nameof(IsCustomerSelected));
            OnPropertyChanged(nameof(IsCustomerNotSelected));
        }

        internal void GetEndDateAndTimeValues()
        {
            if (string.IsNullOrEmpty(SelectedLength)) return;
            var selValue = TheOrderSettings?.Lengths.Find(p => p.Display == SelectedLength);
            if (selValue != null)
            {
                var selValDisplay = selValue.Value;
                GetEndDateTime(selValDisplay);
            }
        }

        private void GetEndDateTime(string dateTimeValue)
        {
            switch (dateTimeValue)
            {
                case "1H":
                    SelectedEndDateTime = SelectedStartDateTime.AddHours(1);
                    break;
                case "2H":
                    SelectedEndDateTime = SelectedStartDateTime.AddHours(2);
                    break;
                case "3H":
                    SelectedEndDateTime = SelectedStartDateTime.AddHours(3);
                    break;
                case "4H":
                    SelectedEndDateTime = SelectedStartDateTime.AddHours(4);
                    break;
                case "5H":
                    SelectedEndDateTime = SelectedStartDateTime.AddHours(5);
                    break;
                case "6H":
                    SelectedEndDateTime = SelectedStartDateTime.AddHours(6);
                    break;
                case "ON":
                    SelectedEndDateTime = SelectedStartDateTime.AddHours(0);
                    break;
                case "OC":
                    SelectedEndDateTime = SelectedStartDateTime.AddHours(0);
                    break;
                case "1D":
                    SelectedEndDateTime = SelectedStartDateTime.AddDays(1);
                    break;
                case "2D":
                    SelectedEndDateTime = SelectedStartDateTime.AddDays(2);
                    break;
                case "3D":
                    SelectedEndDateTime = SelectedStartDateTime.AddDays(3);
                    break;
                case "4D":
                    SelectedEndDateTime = SelectedStartDateTime.AddDays(4);
                    break;
                case "5D":
                    SelectedEndDateTime = SelectedStartDateTime.AddDays(5);
                    break;
                case "6D":
                    SelectedEndDateTime = SelectedStartDateTime.AddDays(6);
                    break;
                case "WK":
                    SelectedEndDateTime = SelectedStartDateTime.AddDays(3);
                    break;
                case "W2":
                    SelectedEndDateTime = SelectedStartDateTime.AddDays(4);
                    break;
                case "1W":
                    SelectedEndDateTime = SelectedStartDateTime.AddDays(7);
                    break;
                case "2W":
                    SelectedEndDateTime = SelectedStartDateTime.AddDays(14);
                    break;
                case "3W":
                    SelectedEndDateTime = SelectedStartDateTime.AddDays(21);
                    break;
                case "1M":
                    SelectedEndDateTime = SelectedStartDateTime.AddMonths(1);
                    break;
                case "E1":
                    SelectedEndDateTime = SelectedStartDateTime.AddHours(0);
                    break;
                case "S1":
                    SelectedEndDateTime = SelectedStartDateTime.AddHours(0);
                    break;
                case "S2":
                    SelectedEndDateTime = SelectedStartDateTime.AddHours(0);
                    break;
                case "S3":
                    SelectedEndDateTime = SelectedStartDateTime.AddHours(0);
                    break;
                case "S4":
                    SelectedEndDateTime = SelectedStartDateTime.AddHours(0);
                    break;
                case "S5":
                    SelectedEndDateTime = SelectedStartDateTime.AddHours(0);
                    break;
                case "S6":
                    SelectedEndDateTime = SelectedStartDateTime.AddHours(0);
                    break;
                case "S7":
                    SelectedEndDateTime = SelectedStartDateTime.AddHours(0);
                    break;
                case "OE":
                    SelectedEndDateTime = SelectedStartDateTime.AddHours(0);
                    break;
                default:
                    SelectedEndDateTime.AddHours(0);
                    break;

            }
            RefreshDateTimeProperties();
        }

        internal async Task<List<string>> CreateNewOrder()
        {
            TheOrderSettings = await NewQuickRentalEntityComponent.GetOrderSettings();
            PopulateMasters();
            OrderUpdate = await NewQuickRentalEntityComponent.GetNewOrderCreationDetail(TheOrderSettings);
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

        internal async Task<bool> VoidOrder()
        {
            return await NewQuickRentalEntityComponent.VoidOrder(CurrentOrder);
        }

        internal async Task<OrderUpdate> UpdateDateValues()
        {
            Indicator = true;
            OrderUpdate responseOrderUpdate = null;
            try
            {
                if (CurrentOrder != null)
                {
                    var selLen = TheOrderSettings?.Lengths.Find(p => p.Display == SelectedLength);
                    if (selLen != null)
                        CurrentOrder.OrderLength = selLen.Value;

                    CurrentOrder.LengthDscr = SelectedLength;

                    var selTax = TheOrderSettings?.TaxCodes.Find(p => p.Display == SelectedTax);
                    if (selTax != null)
                        CurrentOrder.OrderTaxCode = selTax.Value;

                    CurrentOrder.TaxCodeDscr = SelectedTax;

                    CurrentOrder.OrderODte = SelectedStartDateTime;
                    CurrentOrder.OrderDDte = SelectedEndDateTime;

                    CurrentOrder.OrderTaxExempt = ExemptID;

                    var Update = OrderUpdate;
                    Update.Order = CurrentOrder;

                    responseOrderUpdate = await UpdateOrder(Update);
                }
            }
            catch (Exception ex)
            {

            }
            Indicator = false;
            return responseOrderUpdate;
        }

        internal async Task<OrderUpdate> UpdateCurrentOrder(Customer selectedCustomer = null, Tuple<string, string> theNotes = null, OrderUpdate updateOrder = null)
        {
            OrderUpdate responseOrderUpdate = null;
            try
            {
                if (CurrentOrder != null)
                {
                    if (updateOrder == null)
                    {
                        CurrentOrder.Customer = selectedCustomer;
                        CurrentOrder.OrderCustNo = selectedCustomer.CustomerNo;

                        if (theNotes != null)
                        {
                            CurrentOrder.OrderIntNotes = theNotes.Item1;
                            CurrentOrder.OrderNotes = theNotes.Item2;
                        }

                        var Update = OrderUpdate;
                        Update.Order = CurrentOrder;

                        responseOrderUpdate = await UpdateOrder(Update);
                    }
                    else
                    {
                        responseOrderUpdate = await UpdateOrder(updateOrder);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return responseOrderUpdate;
        }
        internal async Task<OrderUpdate> UpdateOrder(OrderUpdate _updateOrder)
        {
            OrderUpdate responseOrderUpdate = null;
            try
            {
                responseOrderUpdate = await NewQuickRentalEntityComponent.UpdateOrder(_updateOrder);
                if (responseOrderUpdate != null && responseOrderUpdate.Order != null)
                {
                    CurrentOrder = responseOrderUpdate.Order;
                    CurrentOrderUpdate.Order = responseOrderUpdate.Order;

                    OnPropertyChanged("CurrentOrder");
                }
            }
            catch (Exception)
            {

            }
            return responseOrderUpdate;
        }
    }
}
