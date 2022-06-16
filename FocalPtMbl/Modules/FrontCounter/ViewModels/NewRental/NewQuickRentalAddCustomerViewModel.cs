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
        public string MyAddress1
        {
            get
            {
                return CustomerToAdd.CustomerAddr1;
            }
            set
            {
                CustomerToAdd.CustomerAddr1 = value;
                OnPropertyChanged("MyAddress1");
            }
        }
        public string MyCustomerCity
        {
            get
            {
                return CustomerToAdd.CustomerCity;
            }
            set
            {
                CustomerToAdd.CustomerCity = value;
                OnPropertyChanged("MyCustomerCity");
            }
        }
        public string MyCustomerStatus
        {
            get
            {
                return CustomerToAdd.CustomerStatus;
            }
            set
            {
                CustomerToAdd.CustomerStatus = value;
                OnPropertyChanged("MyCustomerStatus");
                OnPropertyChanged("CustomerToAdd");
            }
        }
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
            StateList = new string[] { "Select State", "Florida", "Georgia", "MaharashtraAAAABBBBCCCCDDDDMaharashtraAAAABBBBCCCCDDDD", "Uttar Pradesh" };
            CityList = new string[] { "Please Select City", "TX", "NY", "Mumbai", "Pune" };
            PhoneTypeList = new string[] { "Select Type", "Fax", "Cell", "Work", "Home" };
            NotificationTypeList = new string[] { "None", "Email", "SMS", "Both" };
            //ReferredByList = new string[] { "Select Referred Bayaab","AAAA", "BBBB", "CCCC" };
            //CountryList = new string[] { "Select Country", "USA", "India" };
            //StatusList = new string[] { "Select Status", "Prospect", "Good Standing", "On Hold", "Light Hold", "Heavy Hold", "Dormant", "In Collection" };
            //CustomerTypeList = new string[] { "Select Type", "Cash", "Charge", "Miscellaneous", "Open" };
            //CustomerCTypeList = new string[] { "Select Designation", "Construction", "Home Owner", "Major Builder", "Party" };
            //CustomerTermsList = new string[] { "Select Term", "Due On Receipt", "Net 30" };

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
                List<string> itemHolders = new List<string>();

                //Countrys
                itemHolders.Add("Select Country");
                foreach (var item in CustSettings.Countrys)
                {
                    if (item.Display == null) continue;

                    itemHolders.Add(item.Display);
                }
                CountryList = itemHolders.ToArray();

                //CustomerCTypes
                itemHolders.Clear();
                itemHolders.Add("Select Designation");
                foreach (var item in CustSettings.CustomerCTypes)
                {
                    if (item.Display == null) continue;

                    itemHolders.Add(item.Display);
                }
                CustomerCTypeList = itemHolders.ToArray();

                //CustomerFounds
                itemHolders.Clear();
                itemHolders.Add("Select Referred By");
                foreach (var item in CustSettings.CustomerFounds)
                {
                    if (item.Display == null) continue;

                    itemHolders.Add(item.Display);
                }
                ReferredByList = itemHolders.ToArray();

                //CustomerStatus
                itemHolders.Clear();
                itemHolders.Add("Select Status");
                foreach (var item in CustSettings.CustomerStatus)
                {
                    if (item.Display == null) continue;

                    itemHolders.Add(item.Display);
                }
                StatusList = itemHolders.ToArray();

                //CustomerTerms
                itemHolders.Clear();
                itemHolders.Add("Select Term");
                foreach (var item in CustSettings.CustomerTerms)
                {
                    if (item.Display == null) continue;

                    itemHolders.Add(item.Display);
                }
                CustomerTermsList = itemHolders.ToArray();

                //CustomerTypes
                itemHolders.Clear();
                itemHolders.Add("Select Type");
                foreach (var item in CustSettings.CustomerTypes)
                {
                    if (item.Display == null) continue;

                    itemHolders.Add(item.Display);
                }
                CustomerTypeList = itemHolders.ToArray();

                OnPropertyChanged(nameof(CustSettings));
                OnPropertyChanged(nameof(CountryList));
                OnPropertyChanged(nameof(CustomerCTypeList));
                OnPropertyChanged(nameof(ReferredByList));
                OnPropertyChanged(nameof(StatusList));
                OnPropertyChanged(nameof(CustomerTermsList));
                OnPropertyChanged(nameof(CustomerTypeList));

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
