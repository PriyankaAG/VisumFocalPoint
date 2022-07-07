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
        }

        public OrderUpdate orderUpdate;

        public INewQuickRentalEntityComponent NewQuickRentalEntityComponent { get; set; }

        public Int16 SearchIn;
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

        public virtual void populateSearchInList()
        {

        }

        public virtual async Task GetSearchedCustomersInfo(string text)
        {

        }
    }
}
