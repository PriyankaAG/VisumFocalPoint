using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using FocalPtMbl.MainMenu.ViewModels;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.FrontCounter.ViewModels.NewRental
{
    public class NewQuickRentalAddCustomerViewModel : ThemeBaseViewModel
    {
        public INewQuickRentalEntityComponent NewQuickRentalEntityComponent { get; set; }
        public CustomerSettings CustSettings { get; set; }
        public Customer CustomerToAdd { get; set; }
        public string[] ReferredByList { get; set; }
        public string[] CountryList { get; set; }
        public string[] StateList { get; set; }
        public string[] CityList { get; set; }
        public string[] PhoneTypeList { get; set; }
        public string[] StatusList { get; set; }
        public string[] CustomerTypeList { get; set; }
        public string[] CustomerCTypeList { get; set; }
        public string[] CustomerTermsList { get; set; }
        public string[] NotificationTypeList { get; set; }


        public NewQuickRentalAddCustomerViewModel()
        {
            NewQuickRentalEntityComponent = new NewQuickRentalEntityComponent();
       
            CustomerToAdd = new Customer();
            ReferredByList = new string[] { "Select Referred Bayaab","AAAA", "BBBB", "CCCC" };
            CountryList = new string[] { "Select Country", "USA", "India" };
            StateList = new string[] { "Select State", "Florida", "Georgia", "MaharashtraAAAABBBBCCCCDDDDMaharashtraAAAABBBBCCCCDDDD", "Uttar Pradesh" };
            CityList = new string[] { "Please Select City", "TX", "NY", "Mumbai", "Pune" };
            PhoneTypeList = new string[] { "Select Type", "Fax", "Cell", "Work", "Home" };
            StatusList = new string[] { "Select Status", "Prospect", "Good Standing", "On Hold", "Light Hold", "Heavy Hold", "Dormant", "In Collection" };
            CustomerTypeList = new string[] { "Select Type", "Cash", "Charge", "Miscellaneous", "Open" };
            CustomerCTypeList = new string[] { "Select Designation", "Construction", "Home Owner", "Major Builder", "Party" };
            CustomerTermsList = new string[] { "Select Term", "Due On Receipt", "Net 30" };
            NotificationTypeList = new string[] { "None", "Email", "SMS", "Both" };

        }

        public async Task FetchMasters()
        {
            FetchMastersData();
        }

        public async void FetchMastersData()
        {
            try
            {

                Indicator = true;

                CustSettings = await NewQuickRentalEntityComponent.GetCustomerSettings();
                OnPropertyChanged(nameof(CustSettings));

            }
            catch (Exception e)
            {
                //TODO: Log Error
            }
            finally
            {
                Indicator = false;
            }
        }
    }
}
