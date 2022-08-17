using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.FrontCounter.ViewModels.NewRental
{
    public class AddDetailCommonViewModel : ThemeBaseViewModel
    {
        public AddDetailCommonViewModel(string title) : base(title)
        {
            NewQuickRentalEntityComponent = new NewQuickRentalEntityComponent();
            IsRentalAvailMinChgVisible = OrderSettings?.AvailiblityRates?.Contains(RateTypes.Minimum) ?? false;
            IsRentalAvailHourChgVisible = OrderSettings?.AvailiblityRates?.Contains(RateTypes.Hourly) ?? false;
            IsRentalAvailDayChgVisible = OrderSettings?.AvailiblityRates?.Contains(RateTypes.Daily) ?? false;
            IsRentalAvailWeekChgVisible = OrderSettings?.AvailiblityRates?.Contains(RateTypes.Weekly) ?? false;
            IsRentalAvailMonthChgVisible = OrderSettings?.AvailiblityRates?.Contains(RateTypes.Monthly) ?? false;            
        }

        public OrderUpdate orderUpdate;

        public INewQuickRentalEntityComponent NewQuickRentalEntityComponent { get; set; }

        public AvailSearchIns SearchIn;
        public string ItemType;

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

        OrderSettings _orderSettings;
        public OrderSettings OrderSettings
        {
            get
            {
                return _orderSettings;
            }
            set
            {
                _orderSettings = value;
            }
        }

        String[] searchInList;
        public String[] SearchInList
        {
            get => this.searchInList;
            set
            {
                this.searchInList = value;
            }
        }

        private string selectedSearchIn;
        public string SelectedSearchIn
        {
            get => selectedSearchIn;
            set
            {
                this.selectedSearchIn = value;
            }
        }

        private bool _isRentalAvailMinChgVisible;
        public bool IsRentalAvailMinChgVisible
        {
            get => _isRentalAvailMinChgVisible;
            set
            {
                this._isRentalAvailMinChgVisible = value;
            }
        }

        private bool _isRentalAvailHourChgVisible;
        public bool IsRentalAvailHourChgVisible
        {
            get => _isRentalAvailHourChgVisible;
            set
            {
                this._isRentalAvailHourChgVisible = value;
            }
        }

        private bool _isRentalAvailDayChgVisible;
        public bool IsRentalAvailDayChgVisible
        {
            get => _isRentalAvailDayChgVisible;
            set
            {
                this._isRentalAvailDayChgVisible = value;
            }
        }

        private bool _isRentalAvailWeekChgVisible;
        public bool IsRentalAvailWeekChgVisible
        {
            get => _isRentalAvailWeekChgVisible;
            set
            {
                this._isRentalAvailWeekChgVisible = value;
            }
        }

        private bool _isRentalAvailMonthChgVisible;
        public bool IsRentalAvailMonthChgVisible
        {
            get => _isRentalAvailMonthChgVisible;
            set
            {
                this._isRentalAvailMonthChgVisible = value;
            }
        }

        public virtual void populateSearchInList()
        {

        }

        public virtual async Task GetSearchedInfo(string text)
        {

        }
    }
}
