using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using FocalPoint.MainMenu.Models;
using FocalPoint.Validations;
using FocalPoint.Validations.Rules;
using FocalPtMbl.MainMenu.ViewModels;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.FrontCounter.ViewModels.NewRental
{
    public class NewQuickRentalAddCustomerViewModel : ThemeBaseViewModel
    {

        public NewQuickRentalAddCustomerViewModel()
        {
            NewQuickRentalEntityComponent = new NewQuickRentalEntityComponent();

            CustomerToAdd = new Customer();


            _customerName = new ValidatableObject<string>();
            _customerCountry = new ValidatableObject<int>();
            _customerAddr1 = new ValidatableObject<string>();
            _customerCity = new ValidatableObject<int>();
            _customerState = new ValidatableObject<int>();


            AddValidations();

            StateList = new string[] { "Select State", "Florida", "Georgia", "MaharashtraAAAABBBBCCCCDDDDMaharashtraAAAABBBBCCCCDDDD", "Uttar Pradesh" };
            CityList = new string[] { "Please Select City", "TX", "NY", "Mumbai", "Pune" };
            PhoneTypeList = new string[] { "Select Type", "Fax", "Cell", "Work", "Home" };
            NotificationTypeList = new string[] { "None", "Email", "SMS", "Both" };


            //Lets say we load default values
            var tt = "Pune";
            CustomerCityString = tt;
            this.NotifyPropChanged("CustomerCityString");
        }

        #region ==================== Name Section Validations

        private ValidatableObject<string> _customerName;
        public ValidatableObject<string> CustomerName
        {
            get
            {
                return _customerName;
            }
            set
            {
                _customerName = value;
                CustomerToAdd.CustomerName = value.Value;
                OnPropertyChanged(nameof(CustomerName));
            }
        }
        private bool ValidateNameSection()
        {
            CustomerToAdd.CustomerName = _customerName.Value;
            return _customerName.Validate();
        }
        public ICommand ValidateNameSectionCommand => new Command(() => ValidateNameSection());

        #endregion

        #region ==================== Address Section Validations
        public ICommand ValidateAddressSectionCommand => new Command(() => ValidateAddressSection());

        public bool IsAddressSectionValid { get; set; } = true;

        /// <summary>
        /// Country Drop Down
        /// </summary>
        private ValidatableObject<int> _customerCountry;
        public ValidatableObject<int> CustomerCountry
        {
            get
            {
                return _customerCountry;
            }
            set
            {
                _customerCountry = value;

            }
        }
        private string _customerCountryString;
        public string CustomerCountryString
        {
            get
            {
                return _customerCountryString;
            }
            set
            {
                _customerCountryString = value;

                if (CustSettings != null && CustSettings.Countrys != null)
                {
                    var countryVal = CustSettings.Countrys.Find(p => p.Display == value);
                    if (countryVal != null)
                    {
                        CustomerToAdd.CustomerCountry = countryVal.Value;
                    }
                }

                //Find Selected Index
                if (CountryList != null && CountryList.Length > 0)
                {
                    var index = Array.FindIndex(CountryList, r => r == value);
                    CustomerCountry.Value = index;
                    ValidateAddressSection();
                }

                OnPropertyChanged(nameof(CustomerCountryString));
                OnPropertyChanged(nameof(CustomerCountry));
            }
        }

        /// <summary>
        /// City Drop Down
        /// </summary>
        private ValidatableObject<int> _customerCity;
        public ValidatableObject<int> CustomerCity
        {
            get
            {
                return _customerCity;
            }
            set
            {
                _customerCity = value;
                ValidateAddressSection();
                OnPropertyChanged(nameof(CustomerCity));
            }
        }
        private string _customerCityString;
        public string CustomerCityString
        {
            get
            {
                return _customerCityString;
            }
            set
            {
                _customerCityString = value;
                CustomerToAdd.CustomerCity = value;

                //if (CustSettings != null && CustSettings.Countrys != null)
                //{
                //    var countryVal = CustSettings.Countrys.Find(p => p.Display == value);
                //    if (countryVal != null)
                //    {
                //        CustomerToAdd.CustomerCountry = countryVal.Value;
                //    }
                //}

                //Find Selected Index
                if (CityList != null && CityList.Length > 0)
                {
                    var index = Array.FindIndex(CityList, r => r == value);
                    CustomerCity.Value = index;
                    ValidateAddressSection();
                }

                OnPropertyChanged(nameof(CustomerCountryString));
                OnPropertyChanged(nameof(CustomerCountry));
            }
        }

        /// <summary>
        /// State Drop Down
        /// </summary>
        private ValidatableObject<int> _customerState;
        public ValidatableObject<int> CustomerState
        {
            get
            {
                return _customerState;
            }
            set
            {
                _customerState = value;
                ValidateAddressSection();
                OnPropertyChanged(nameof(CustomerState));
            }
        }
        private string _customerStateString;
        public string CustomerStateString
        {
            get
            {
                return _customerStateString;
            }
            set
            {
                _customerStateString = value;
                CustomerToAdd.CustomerState = value;

                //if (CustSettings != null && CustSettings.Countrys != null)
                //{
                //    var countryVal = CustSettings.Countrys.Find(p => p.Display == value);
                //    if (countryVal != null)
                //    {
                //        CustomerToAdd.CustomerCountry = countryVal.Value;
                //    }
                //}

                //Find Selected Index
                if (StateList != null && StateList.Length > 0)
                {
                    var index = Array.FindIndex(StateList, r => r == value);
                    CustomerState.Value = index;
                    ValidateAddressSection();
                }

                OnPropertyChanged(nameof(CustomerStateString));
                OnPropertyChanged(nameof(CustomerState));
            }
        }


        private ValidatableObject<string> _customerAddr1;
        public ValidatableObject<string> CustomerAddr1
        {
            get
            {
                return _customerAddr1;
            }
            set
            {
                _customerAddr1 = value;
                CustomerToAdd.CustomerAddr1 = value.Value;
            }
        }

        private bool ValidateAddressSection()
        {
            var countryValid = _customerCountry.Validate();

            var addrValid = _customerAddr1.Validate();
            CustomerToAdd.CustomerAddr1 = _customerAddr1.Value;

            var stateValid = _customerState.Validate();
            var cityValid = _customerCity.Validate();

            IsAddressSectionValid = countryValid && addrValid && stateValid && cityValid;
            OnPropertyChanged("IsAddressSectionValid");
            return IsAddressSectionValid;
        }
        #endregion

        public bool ValidateData() => Validate();

        private bool Validate()
        {
            bool isValidNameSection = ValidateNameSection();
            bool isValidAddressSection = ValidateAddressSection();

            return isValidNameSection && isValidAddressSection;
        }

        public void NotifyPropChanged(string propName = "CustomerToAdd")
        {
            OnPropertyChanged(propName);
        }
        public INewQuickRentalEntityComponent NewQuickRentalEntityComponent { get; set; }
        public CustomerSettings CustSettings { get; set; }
        private Customer _customerToAdd;
        public Customer CustomerToAdd
        {
            get
            {
                return _customerToAdd;
            }
            set
            {
                _customerToAdd = value;
                OnPropertyChanged("CustomerToAdd");
            }
        }
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


        private void AddValidations()
        {
            _customerName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Customer Name can not be empty." });
            _customerCountry.Validations.Add(new IsComboboxNotSelected { ValidationMessage = "Country can not be empty." });
            _customerAddr1.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Address can not be empty." });
            _customerCity.Validations.Add(new IsComboboxNotSelected { ValidationMessage = "City can not be empty." });
            _customerState.Validations.Add(new IsComboboxNotSelected { ValidationMessage = "State can not be empty." });

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
