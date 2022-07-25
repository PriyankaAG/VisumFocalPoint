using System;
using System.Linq;
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
using System.Text.RegularExpressions;
using FocalPoint.Utils;

namespace FocalPoint.Modules.FrontCounter.ViewModels.NewRental
{
    public class NewQuickRentalAddCustomerViewModel : ThemeBaseViewModel
    {
        #region ==================== Properties

        public enum ReminderTypes
        {
            None = 0,
            Email = 1,
            SMS = 2,
            Both = 3
        }
        public byte NotificationReminderToByte(string val)
        {
            byte res = byte.MinValue;
            try
            {
                ReminderTypes selected;
                if (Enum.TryParse<ReminderTypes>(val, out selected))
                {
                    res = Convert.ToByte(selected);
                }
            }
            catch (Exception)
            {

            }

            return res;
        }
        string _customerOEMail;
        public string CustomerOEMail
        {
            get
            {
                return _customerOEMail;
            }
            set
            {
                _customerOEMail = value;
                CustomerToAdd.CustomerOEMail = NotificationReminderToByte(value);
            }
        }
        string _CustomerCEMail;
        public string CustomerCEMail
        {
            get
            {
                return _CustomerCEMail;
            }
            set
            {
                _CustomerCEMail = value;
                CustomerToAdd.CustomerCEMail = NotificationReminderToByte(value);
            }
        }
        string _CustomerROHEMail;
        public string CustomerROHEMail
        {
            get
            {
                return _CustomerROHEMail;
            }
            set
            {
                _CustomerROHEMail = value;
                CustomerToAdd.CustomerROHEMail = NotificationReminderToByte(value);
            }
        }
        string _CustomerWOCEMail;
        public string CustomerWOCEMail
        {
            get
            {
                return _CustomerWOCEMail;
            }
            set
            {
                _CustomerWOCEMail = value;
                CustomerToAdd.CustomerWOCEMail = NotificationReminderToByte(value);
            }
        }
        string _CustomerWOFEMail;
        public string CustomerWOFEMail
        {
            get
            {
                return _CustomerWOFEMail;
            }
            set
            {
                _CustomerWOFEMail = value;
                CustomerToAdd.CustomerWOFEMail = NotificationReminderToByte(value);
            }
        }
        string _CustomerReminder;
        public string CustomerReminder
        {
            get
            {
                return _CustomerReminder;
            }
            set
            {
                _CustomerReminder = value;
                CustomerToAdd.CustomerReminder = NotificationReminderToByte(value);
            }
        }
        string _CustomerReminderEvent;
        public string CustomerReminderEvent
        {
            get
            {
                return _customerOEMail;
            }
            set
            {
                _CustomerReminderEvent = value;
                CustomerToAdd.CustomerReminderEvent = NotificationReminderToByte(value);
            }
        }
        string _phoneNumberStr;
        public string PhoneNumberStr
        {
            get
            {
                return _phoneNumberStr;
            }
            set
            {
                PhoneNumber.Value = value;
                _phoneNumberStr = fomatPhoneNumber(value);
                OnPropertyChanged("PhoneNumberStr");
                OnPropertyChanged("PhoneNumber");
            }
        }
        string _phoneNumber2;
        public string PhoneNumber2
        {
            get
            {
                return _phoneNumber2;
            }
            set
            {
                CustomerToAdd.CustomerPhone2 = value;
                _phoneNumber2 = fomatPhoneNumber(value);
                OnPropertyChanged("PhoneNumber2");
            }
        }
        string _phoneNumber3;
        public string PhoneNumber3
        {
            get
            {
                return _phoneNumber3;
            }
            set
            {
                CustomerToAdd.CustomerPhone3 = value;
                _phoneNumber3 = fomatPhoneNumber(value);
                OnPropertyChanged("PhoneNumber3");
            }
        }
        string _phoneNumberSMS;
        public string PhoneNumberSMS
        {
            get
            {
                return _phoneNumberSMS;
            }
            set
            {
                CustomerToAdd.CustomerSMSNumber = value;
                _phoneNumberSMS = fomatPhoneNumber(value);
                OnPropertyChanged("PhoneNumberSMS");
            }
        }
        public bool IsPageLoaded { get; set; }
        bool _isPrintAlsoIfSent;
        public bool IsPrintAlsoIfSent
        {
            get
            {
                return _isPrintAlsoIfSent;
            }
            set
            {
                _isPrintAlsoIfSent = value;
                OnPropertyChanged("IsPrintAlsoIfSent");
            }
        }
        bool _isCountryLoading;
        public bool IsCountryLoading
        {
            get
            {
                return _isCountryLoading;
            }
            set
            {
                _isCountryLoading = value;
                OnPropertyChanged("IsCountryLoading");
            }
        }
        bool _isStateLoading;
        public bool IsStateLoading
        {
            get
            {
                return _isStateLoading;
            }
            set
            {
                _isStateLoading = value;
                OnPropertyChanged("IsStateLoading");
            }
        }
        bool _isCityLoading;
        public bool IsCityLoading
        {
            get
            {
                return _isCityLoading;
            }
            set
            {
                _isCityLoading = value;
                OnPropertyChanged("IsCityLoading");
            }
        }
        public INewQuickRentalEntityComponent NewQuickRentalEntityComponent { get; set; }
        public CustomerSettings CustSettings { get; set; }
        public List<DisplayValueString> StatesAPIResult { get; set; }
        public List<string> CityAPIResult { get; set; }
        public List<string> BillingCityAPIResult { get; set; }
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
        public string[] BillingCityList { get; set; }
        public string[] PhoneTypeList { get; set; }
        public string[] StatusList { get; set; }
        public string[] CustomerTypeList { get; set; }
        public string[] CustomerCTypeList { get; set; }
        public string[] CustomerTermsList { get; set; }
        public string[] NotificationTypeList { get; set; }
        public List<DisplayValueString> PhoneTypes { get; set; } = new List<DisplayValueString>()
        {
            new DisplayValueString(){ Display="Fax",Value ="F" },
            new DisplayValueString(){ Display="Cell",Value ="C" },
            new DisplayValueString(){ Display="Work",Value ="W" },
            new DisplayValueString(){ Display="Home",Value ="H" }
        };
        public string StateLabel { get; set; } = "State *";
        public string ZipLabel { get; set; } = "Zip *";
        string _phoneTypeDefault1;
        public string PhoneTypeDefault1
        {
            get
            {
                return _phoneTypeDefault1;
            }
            set
            {
                _phoneTypeDefault1 = value;
                CustomerToAdd.CustomerPhoneType = ReturnPhoneType(value);
                OnPropertyChanged("PhoneTypeDefault1");
            }
        }
        string _phoneTypeDefault2;
        public string PhoneTypeDefault2
        {
            get
            {
                return _phoneTypeDefault2;
            }
            set
            {
                _phoneTypeDefault2 = value;
                CustomerToAdd.CustomerPhoneType2 = ReturnPhoneType(value);
                OnPropertyChanged("PhoneTypeDefault2");
            }
        }
        string _phoneTypeDefault3;
        public string PhoneTypeDefault3
        {
            get
            {
                return _phoneTypeDefault3;
            }
            set
            {
                _phoneTypeDefault3 = value;
                CustomerToAdd.CustomerPhoneType3 = ReturnPhoneType(value);
                OnPropertyChanged("PhoneTypeDefault3");
            }
        }
        public string ReturnPhoneType(string DisplayType)
        {
            if (CustSettings != null && PhoneTypes != null)
            {
                var phoneVal = PhoneTypes.Find(p => p.Display == DisplayType);
                if (phoneVal != null)
                {
                    return phoneVal.Value;
                }
            }
            return "";
        }


        #endregion

        #region ==================== Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public NewQuickRentalAddCustomerViewModel()
        {
            NewQuickRentalEntityComponent = new NewQuickRentalEntityComponent();

            CustomerToAdd = new Customer();

            _customerName = new ValidatableObject<string>();
            _customerCountry = new ValidatableObject<int>();
            _customerAddr1 = new ValidatableObject<string>();
            _customerCity = new ValidatableObject<int>();
            _customerState = new ValidatableObject<int>();
            _customerZip = new ValidatableObject<string>();

            _customerBillingCity = new ValidatableObject<int>();
            _customerBillingState = new ValidatableObject<int>();
            _customerBillingAddr1 = new ValidatableObject<string>();
            _customerBillingZip = new ValidatableObject<string>();

            _phoneNumber = new ValidatableObject<string>();

            _customerCTypeValidation = new ValidatableObject<int>();

            _licenceExpiration = new ValidatableObject<string>();
            _licenceState = new ValidatableObject<int>();

            AddValidations();


        }

        #endregion

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
            if (!IsPageLoaded) return true;

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
                int index = 0;
                if (CountryList != null && CountryList.Length > 0)
                {
                    index = Array.FindIndex(CountryList, r => r == value);
                    CustomerCountry.Value = index;
                    ValidateAddressSection();
                }

                if (CustomerToAdd.CustomerCountry == 2)
                {
                    StateLabel = "Prov *";
                    ZipLabel = "Postal *";
                }
                else
                {
                    StateLabel = "State *";
                    ZipLabel = "Zip *";
                }

                //Whenever country is changed, change its states
                if (!string.IsNullOrEmpty(value))
                    GetStatesFromCountry(CustomerToAdd.CustomerCountry);

                OnPropertyChanged(nameof(CustomerCountryString));
                OnPropertyChanged(nameof(CustomerCountry));
                OnPropertyChanged(nameof(StateLabel));
                OnPropertyChanged(nameof(ZipLabel));
            }
        }
        public async void GetStatesFromCountry(int country)
        {
            IsStateLoading = true;
            StatesAPIResult = await NewQuickRentalEntityComponent.GetStates(country.ToString());
            if (StatesAPIResult == null)
            {
                StateList = new string[] {
                "Select State"
                };
                return;
            }
            var result = StatesAPIResult.Select(p => p.Display).ToList();
            result.Insert(0, "Select State");
            StateList = result.ToArray();
            OnPropertyChanged("StateList");
            IsStateLoading = false;
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

                //Find Selected Index
                if (CityList != null && CityList.Length > 0)
                {
                    var index = Array.FindIndex(CityList, r => r == value);
                    CustomerCity.Value = index;
                    ValidateAddressSection();
                }

                OnPropertyChanged(nameof(CustomerCityString));
                OnPropertyChanged(nameof(CustomerCity));
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
                //CustomerToAdd.CustomerState = value;

                if (StatesAPIResult != null && StatesAPIResult.Count > 0)
                {
                    var stateVal = StatesAPIResult.Find(p => p.Display == value);
                    if (stateVal != null)
                    {
                        CustomerToAdd.CustomerState = stateVal.Value;
                    }
                }

                //Find Selected Index
                if (StateList != null && StateList.Length > 0)
                {
                    var index = Array.FindIndex(StateList, r => r == value);
                    CustomerState.Value = index;
                    ValidateAddressSection();
                }

                //Whenever country is changed, change its states
                if (!string.IsNullOrEmpty(value))
                    GetCityFromState(CustomerToAdd.CustomerCountry, CustomerToAdd.CustomerState);

                OnPropertyChanged(nameof(CustomerStateString));
                OnPropertyChanged(nameof(CustomerState));
            }
        }
        public async void GetCityFromState(int country, string state)
        {
            IsCityLoading = true;
            var res = await NewQuickRentalEntityComponent.GetCityByState(country.ToString(), state);
            if (res == null) return;
            if (res.Cities == null || res.Cities.Count == 0)
                CityAPIResult = new List<string>();
            else
                CityAPIResult = res.Cities;

            CityAPIResult.Insert(0, "Select City");
            CityList = CityAPIResult.ToArray();
            OnPropertyChanged("CityList");
            IsCityLoading = false;
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

        private ValidatableObject<string> _customerZip;
        public ValidatableObject<string> CustomerZip
        {
            get
            {
                return _customerZip;
            }
            set
            {
                _customerZip = value;
                CustomerToAdd.CustomerZip = value.Value;
            }
        }
        public string ZipCodeStr
        {
            get
            {
                return CustomerZip.Value;
            }
            set
            {
                CustomerZip.Value = fomatZipCode(value);
                OnPropertyChanged("ZipCodeStr");
                OnPropertyChanged("CustomerZip");
            }
        }
        private bool ValidateAddressSection()
        {
            if (!IsPageLoaded) return true;

            var countryValid = _customerCountry.Validate();

            var addrValid = _customerAddr1.Validate();
            CustomerToAdd.CustomerAddr1 = _customerAddr1.Value;

            var zipValid = _customerZip.Validate();
            CustomerToAdd.CustomerZip = _customerZip.Value;

            var stateValid = _customerState.Validate();
            var cityValid = _customerCity.Validate();

            IsAddressSectionValid = countryValid && addrValid && stateValid && cityValid;
            OnPropertyChanged("IsAddressSectionValid");
            return IsAddressSectionValid;
        }

        #endregion

        #region ==================== Billing Address Section Validations
        public ICommand ValidateBillingAddressSectionCommand => new Command(() => ValidateBillingAddressSection());

        public bool IsBillingAddressSectionValid { get; set; } = true;

        /// <summary>
        /// City Drop Down
        /// </summary>
        private ValidatableObject<int> _customerBillingCity;
        public ValidatableObject<int> CustomerBillingCity
        {
            get
            {
                return _customerBillingCity;
            }
            set
            {
                _customerBillingCity = value;
            }
        }
        private string _customerBillingCityString;
        public string CustomerBillingCityString
        {
            get
            {
                return _customerBillingCityString;
            }
            set
            {
                _customerBillingCityString = value;
                CustomerToAdd.CustomerBillCity = value;

                //if (CustSettings != null && CustSettings.Countrys != null)
                //{
                //    var countryVal = CustSettings.Countrys.Find(p => p.Display == value);
                //    if (countryVal != null)
                //    {
                //        CustomerToAdd.CustomerCountry = countryVal.Value;
                //    }
                //}

                //Find Selected Index
                if (BillingCityList != null && BillingCityList.Length > 0)
                {
                    var index = Array.FindIndex(BillingCityList, r => r == value);
                    CustomerBillingCity.Value = index;
                    ValidateBillingAddressSection();
                }

                OnPropertyChanged(nameof(_customerBillingCityString));
                OnPropertyChanged(nameof(CustomerBillingCity));
            }
        }

        /// <summary>
        /// State Drop Down
        /// </summary>
        private ValidatableObject<int> _customerBillingState;
        public ValidatableObject<int> CustomerBillingState
        {
            get
            {
                return _customerBillingState;
            }
            set
            {
                _customerBillingState = value;
            }
        }
        private string _customerBillingStateString;
        public string CustomerBillingStateString
        {
            get
            {
                return _customerBillingStateString;
            }
            set
            {
                _customerBillingStateString = value;
                //CustomerToAdd.CustomerState = value;

                if (StatesAPIResult != null && StatesAPIResult.Count > 0)
                {
                    var stateVal = StatesAPIResult.Find(p => p.Display == value);
                    if (stateVal != null)
                    {
                        CustomerToAdd.CustomerBillState = stateVal.Value;
                    }
                }

                //Find Selected Index
                if (StateList != null && StateList.Length > 0)
                {
                    var index = Array.FindIndex(StateList, r => r == value);
                    CustomerBillingState.Value = index;
                    ValidateBillingAddressSection();
                }

                //Whenever country is changed, change its states
                if (!string.IsNullOrEmpty(value))
                    GetBillingCityFromState(CustomerToAdd.CustomerCountry, CustomerToAdd.CustomerBillState);

                OnPropertyChanged(nameof(CustomerStateString));
                OnPropertyChanged(nameof(CustomerState));
            }
        }
        public async void GetBillingCityFromState(int country, string state)
        {
            IsCityLoading = true;
            var res = await NewQuickRentalEntityComponent.GetCityByState(country.ToString(), state);
            if (res == null) return;
            if (res.Cities == null || res.Cities.Count == 0)
                BillingCityAPIResult = new List<string>();
            else
                BillingCityAPIResult = res.Cities;

            BillingCityAPIResult.Insert(0, "Select City");
            BillingCityList = BillingCityAPIResult.ToArray();
            OnPropertyChanged("BillingCityList");
            IsCityLoading = false;
        }


        private ValidatableObject<string> _customerBillingAddr1;
        public ValidatableObject<string> CustomerBillingAddr1
        {
            get
            {
                return _customerBillingAddr1;
            }
            set
            {
                _customerBillingAddr1 = value;
                CustomerToAdd.CustomerBillAddr1 = value.Value;
            }
        }


        private ValidatableObject<string> _customerBillingZip;
        public ValidatableObject<string> CustomerBillingZip
        {
            get
            {
                return _customerBillingZip;
            }
            set
            {
                _customerBillingZip = value;
                CustomerToAdd.CustomerBillZip = value.Value;
            }
        }
        public string BillingZipCodeStr
        {
            get
            {
                return CustomerBillingZip.Value;
            }
            set
            {
                CustomerBillingZip.Value = fomatZipCode(value);
                OnPropertyChanged("BillingZipCodeStr");
                OnPropertyChanged("CustomerBillingZip");
            }
        }
        private bool ValidateBillingAddressSection()
        {
            if (!IsPageLoaded) return true;

            var countryValid = _customerCountry.Validate();

            var addrValid = _customerBillingAddr1.Validate();
            CustomerToAdd.CustomerBillAddr1 = _customerBillingAddr1.Value;

            var zipValid = _customerBillingZip.Validate();
            CustomerToAdd.CustomerBillZip = _customerBillingZip.Value;

            var stateValid = _customerBillingState.Validate();
            var cityValid = _customerBillingCity.Validate();

            IsBillingAddressSectionValid = countryValid && addrValid && stateValid && cityValid;
            OnPropertyChanged("IsBillingAddressSectionValid");
            return IsBillingAddressSectionValid;
        }

        #endregion

        #region ==================== Phone Number Validations

        private ValidatableObject<string> _phoneNumber;
        public ValidatableObject<string> PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                _phoneNumber = value;
                CustomerToAdd.CustomerPhone = value.Value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }
        private bool ValidatePhoneSection()
        {
            if (!IsPageLoaded) return true;

            CustomerToAdd.CustomerPhone = _phoneNumber.Value;
            return _phoneNumber.Validate();
        }
        public ICommand ValidatePhoneSectionCommand => new Command(() => ValidatePhoneSection());

        #endregion

        #region  ==================== Account/Tax Info Validations

        string _custStatus;
        public string CustStatus
        {
            get
            {
                return _custStatus;
            }
            set
            {
                _custStatus = value;
                CustomerToAdd.CustomerStatus = ReturnStatus(value);
                OnPropertyChanged("CustStatus");
            }
        }
        public string ReturnStatus(string DisplayType)
        {
            if (CustSettings != null && CustSettings.CustomerStatus != null)
            {
                var statusVal = CustSettings.CustomerStatus.Find(p => p.Display == DisplayType);
                if (statusVal != null)
                {
                    return statusVal.Value;
                }
            }
            return "";
        }
        string _custType;
        public string CustType
        {
            get
            {
                return _custType;
            }
            set
            {
                _custType = value;
                CustomerToAdd.CustomerType = ReturnType(value);
                OnPropertyChanged("CustType");
            }
        }
        public string ReturnType(string DisplayType)
        {
            if (CustSettings != null && CustSettings.CustomerTypes != null)
            {
                var typeVal = CustSettings.CustomerTypes.Find(p => p.Display == DisplayType);
                if (typeVal != null)
                {
                    return typeVal.Value;
                }
            }
            return "";
        }
        string _custCType;
        public string CustCType
        {
            get
            {
                return _custCType;
            }
            set
            {
                _custCType = value;
                CustomerToAdd.CustomerCType = ReturnCType(value);
                _customerCTypeValidation.Value = CustomerToAdd.CustomerCType;
                ValidateAccountSection();
                OnPropertyChanged("CustCType");
            }
        }
        string _custCTypeLable = "Designation";
        public string CustCTypeLable
        {
            get
            {
                return _custCTypeLable;
            }
            set
            {
                _custCTypeLable = value;

            }
        }
        public short ReturnCType(string DisplayType)
        {
            if (CustSettings != null && CustSettings.CustomerCTypes != null)
            {
                var typeVal = CustSettings.CustomerCTypes.Find(p => p.Display == DisplayType);
                if (typeVal != null)
                {
                    return typeVal.Value;
                }
            }
            return 0;
        }
        private bool ValidateAccountSection()
        {
            if (!IsPageLoaded) return true;

            var countryValid = _customerCTypeValidation.Validate();

            return countryValid;
        }

        private ValidatableObject<int> _customerCTypeValidation;
        public ValidatableObject<int> CustomerCTypeValidation
        {
            get
            {
                return _customerCTypeValidation;
            }
            set
            {
                _customerCTypeValidation = value;

            }
        }

        string _custTerm;
        public string CustTerm
        {
            get
            {
                return _custTerm;
            }
            set
            {
                _custTerm = value;
                CustomerToAdd.CustomerTerms = ReturnTerm(value);
                OnPropertyChanged("CustTerm");
            }
        }
        public short ReturnTerm(string DisplayType)
        {
            if (CustSettings != null && CustSettings.CustomerTerms != null)
            {
                var termVal = CustSettings.CustomerTerms.Find(p => p.Display == DisplayType);
                if (termVal != null)
                {
                    return termVal.Value;
                }
            }
            return 0;
        }

        #endregion

        #region  ==================== Personal Information Validations

        public bool IsPersonalInfoValid { get; set; } = true;
        private string _licenceNumberDisplay;
        public string LicenceNumberDisplay
        {
            get
            {
                return _licenceNumberDisplay;
            }
            set
            {
                _licenceNumberDisplay = value;
                CustomerToAdd.CustomerDrLic = value;

                if (string.IsNullOrEmpty(_licenceNumberDisplay) || string.IsNullOrWhiteSpace(_licenceNumberDisplay))
                {

                    _licenceState.Validations.Clear();
                    _licenceExpiration.Validations.Clear();
                    LicenceStateLabel = "License State";
                    LicenceExpirationLabel = "License Expiration";
                }
                else
                {
                    _licenceState.Validations.Add(new IsComboboxNotSelected { ValidationMessage = "License State can not be empty." });
                    _licenceExpiration.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Expiration can not be empty." });
                    LicenceStateLabel = "License State *";
                    LicenceExpirationLabel = "License Expiration *";
                }

                OnPropertyChanged("LicenceState");
                OnPropertyChanged("LicenceExpiration");
                OnPropertyChanged("LicenceStateLabel");
                OnPropertyChanged("LicenceExpirationLabel");

                ValidatePersonalInfoSection();
            }
        }

        private ValidatableObject<string> _licenceExpiration;
        public ValidatableObject<string> LicenceExpiration
        {
            get
            {
                return _licenceExpiration;
            }
            set
            {
                _licenceExpiration = value;
                OnPropertyChanged(nameof(LicenceExpiration));
            }
        }
        DateTime _licenceExpirationDateTime;
        public DateTime LicenceExpirationDateTime
        {
            get
            {
                return _licenceExpirationDateTime;
            }
            set
            {
                _licenceExpirationDateTime = value;
                CustomerToAdd.CustomerDLEDte = value;
                LicenceExpiration.Value = value.ToString();
                OnPropertyChanged(nameof(LicenceExpirationDateTime));
            }
        }
        public string LicenceExpirationLabel { get; set; } = "License Expiration";

        private ValidatableObject<int> _licenceState;
        public ValidatableObject<int> LicenceState
        {
            get
            {
                return _licenceState;
            }
            set
            {
                _licenceState = value;
            }
        }
        private string _licenceStateDisplay;
        public string LicenceStateDisplay
        {
            get
            {
                return _licenceStateDisplay;
            }
            set
            {
                _licenceStateDisplay = value;

                if (StatesAPIResult != null && StatesAPIResult.Count > 0)
                {
                    var stateVal = StatesAPIResult.Find(p => p.Display == value);
                    if (stateVal != null)
                    {
                        CustomerToAdd.CustomerDrLicSt = stateVal.Value;
                    }
                }

                //Find Selected Index
                if (StateList != null && StateList.Length > 0)
                {
                    var index = Array.FindIndex(StateList, r => r == value);
                    LicenceState.Value = index;
                    ValidatePersonalInfoSection();
                }

                OnPropertyChanged(nameof(LicenceStateDisplay));
                OnPropertyChanged(nameof(LicenceState));
            }
        }
        public string LicenceStateLabel { get; set; } = "License State";

        private bool ValidatePersonalInfoSection()
        {
            if (!IsPageLoaded) return true;

            CustomerToAdd.CustomerDrLic = _licenceNumberDisplay;
            //CustomerToAdd.CustomerDrLicSt = _licenceStateDisplay;

            bool expChk = _licenceExpiration.Validate();
            bool stateChk = _licenceState.Validate();

            IsPersonalInfoValid = expChk && stateChk;
            OnPropertyChanged("IsPersonalInfoValid");

            return IsPersonalInfoValid;
        }
        public ICommand ValidateInfoSectionCommand => new Command(() => ValidatePersonalInfoSection());

        #endregion

        #region  ==================== General Functions

        public void SetDefaultValues()
        {
            var ctryIndex = CustSettings.Defaults.CustomerCountry;
            var ctrFound = CustSettings.Countrys.Find(p => p.Value == ctryIndex);
            if (ctrFound != null)
            {
                CustomerCountryString = ctrFound.Display;
            }

            var phnVal = CustSettings.Defaults.CustomerPhoneType;
            var phnFound = PhoneTypes.Find(p => p.Value == phnVal);
            if (phnFound != null)
            {
                PhoneTypeDefault1 = phnFound.Display;
            }

            var phnVal2 = CustSettings.Defaults.CustomerPhoneType2;
            var phnFound2 = PhoneTypes.Find(p => p.Value == phnVal2);
            if (phnFound2 != null)
            {
                PhoneTypeDefault2 = phnFound2.Display;
            }

            var phnVal3 = CustSettings.Defaults.CustomerPhoneType3;
            var phnFound3 = PhoneTypes.Find(p => p.Value == phnVal3);
            if (phnFound3 != null)
            {
                PhoneTypeDefault3 = phnFound3.Display;
            }

            var statusVal = CustSettings.Defaults.CustomerStatus;
            var statusFound = CustSettings.CustomerStatus.Find(p => p.Value == statusVal);
            if (statusFound != null)
            {
                CustStatus = statusFound.Display;
            }

            var typeVal = CustSettings.Defaults.CustomerType;
            var typeFound = CustSettings.CustomerTypes.Find(p => p.Value == typeVal);
            if (typeFound != null)
            {
                CustType = typeFound.Display;
            }

            var termVal = CustSettings.Defaults.CustomerTerms;
            var termFound = CustSettings.CustomerTerms.Find(p => p.Value == termVal);
            if (termFound != null)
            {
                CustTerm = termFound.Display;
            }


            CustomerToAdd.CustomerDW = CustSettings.Defaults.CustomerDW;
        }

        public async Task FetchMasters()
        {
            await FetchMastersData();
        }

        public async Task<string> CheckPhoneNumber()
        {
            var str = await NewQuickRentalEntityComponent.CheckPhoneNumber(CustomerToAdd.CustomerPhone);
            return str;
        }
        public async Task<string> CheckCustomerName()
        {
            var str = await NewQuickRentalEntityComponent.CheckCustomerName(CustomerToAdd.CustomerFName, CustomerToAdd.CustomerLName);
            return str;
        }
        public async Task<string> CheckLicenseNumber()
        {
            var str = await NewQuickRentalEntityComponent.CheckDrivLicID(CustomerToAdd.CustomerDrLic);//CustomerToAdd.CustomerPhone);
            return str;
        }
        public async Task<Customer> AddCustomer()
        {
           var result = await NewQuickRentalEntityComponent.AddCustomer(CustomerToAdd);
            return result;
        }
        public async Task FetchMastersData()
        {
            try
            {

                Indicator = IsCountryLoading = true;

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

                CityList = new string[] { "Select City" };
                BillingCityList = new string[] { "Select City" };
                StateList = new string[] { "Select State" };

                PhoneTypeList = new string[] { "Select Type", "Fax", "Cell", "Work", "Home" };
                NotificationTypeList = new string[] { "None", "Email", "SMS", "Both" };

                OnPropertyChanged(nameof(CustSettings));
                OnPropertyChanged(nameof(CountryList));
                OnPropertyChanged(nameof(CustomerCTypeList));
                OnPropertyChanged(nameof(ReferredByList));
                OnPropertyChanged(nameof(StatusList));
                OnPropertyChanged(nameof(CustomerTermsList));
                OnPropertyChanged(nameof(CustomerTypeList));

                OnPropertyChanged(nameof(StateList));
                OnPropertyChanged(nameof(CityList));
                OnPropertyChanged(nameof(BillingCityList));
                OnPropertyChanged(nameof(PhoneTypeList));
                OnPropertyChanged(nameof(NotificationTypeList));

                var res = CustSettings.Controls.CustomerCTypeReq;
                if (res)
                {
                    _customerCTypeValidation.Validations.Add(new IsComboboxNotSelected { ValidationMessage = "Type can not be empty." });

                    CustCTypeLable = "Designation *";

                    OnPropertyChanged("CustCTypeLable");
                }
            }
            catch (Exception e)
            {
                //TODO: Log Error
            }
            finally
            {
                Indicator = IsCountryLoading = false;

            }
        }

        public string fomatZipCode(string input)
        {
            var formattedZip = input;

            if (CustomerToAdd.CustomerCountry == 1)
                formattedZip = Regex.Replace(formattedZip, @"(\d{5})(\d{4})", @"$1-$2");
            else
                formattedZip = Regex.Replace(formattedZip, @"([A-Z]\d[A-Z])(\d[A-Z]\d)", @"$1 $2");

            return formattedZip;
        }

        private string fomatPhoneNumber(string input)
        {
            var formattedNo = input;

            if (formattedNo != null && formattedNo.Length > 0)
                formattedNo = Regex.Replace(formattedNo, @"(\d{3})(\d{3})(\d{4})", "($1)$2-$3");

            return formattedNo;
        }

        public void FormatCustomerName()
        {
            var CustName = CustomerToAdd.CustomerName == null ? "" : CustomerToAdd.CustomerName;
            var FName = CustomerToAdd.CustomerFName == null ? "" : CustomerToAdd.CustomerFName;
            var MName = CustomerToAdd.CustomerMI == null ? "" : CustomerToAdd.CustomerMI;
            var LName = CustomerToAdd.CustomerLName == null ? "" : CustomerToAdd.CustomerLName;

            if (CustName.Length == 0)
            {
                if (CustSettings.Controls.FormatLastNameFirst)
                {
                    if (LName.Length != 0 && FName.Length != 0)
                    {
                        CustName = new string((LName + ", " + FName).Take(35).ToArray());
                        if (MName.Length != 0 & MName.Length < 34)
                            CustName = CustName + " " + MName;
                    }
                }
                else if (LName.Length != 0 & FName.Length != 0)
                {
                    CustName = FName;
                    if (MName.Length != 0)
                        CustName = new string((CustName + " " + MName + " " + LName).Take(35).ToArray());
                    else
                        CustName = new string((CustName + " " + LName).Take(35).ToArray());
                }
                CustomerName.Value = CustName;
                OnPropertyChanged("CustomerName");
            }
        }

        public bool CheckZipCode()
        {
            if (!CustomerToAdd.CustomerZip.HasData() || !CustomerToAdd.CustomerBillZip.HasData())
            {
                return false;
            }

            bool res1;
            bool res2;
            if (CustomerToAdd.CustomerCountry == 1)
            {
                res1 = Regex.IsMatch(CustomerToAdd.CustomerZip, @"\d{5}(-\d{4})?");
                res2 = Regex.IsMatch(CustomerToAdd.CustomerBillZip, @"\d{5}(-\d{4})?");
                return res1 && res2;
            }
            else
            {
                res1 = Regex.IsMatch(CustomerToAdd.CustomerZip, @"[A-Z]\d[A-Z] \d[A-Z]\d");
                res2 = Regex.IsMatch(CustomerToAdd.CustomerBillZip, @"[A-Z]\d[A-Z] \d[A-Z]\d");
                return res1 && res2;
            }
        }
        #endregion

        #region  ==================== General Validation Functions

        public bool ValidateData() => Validate();

        private bool Validate()
        {
            bool isValidNameSection = ValidateNameSection();
            bool isValidAddressSection = ValidateAddressSection();
            bool isValidBillingAddressSection = ValidateBillingAddressSection();
            bool isValidPhoneSection = ValidatePhoneSection();
            bool isValidAccountSection = ValidateAccountSection();
            bool isValidPersonalInfoSection = ValidatePersonalInfoSection();

            return isValidNameSection && isValidAddressSection && isValidBillingAddressSection && isValidPhoneSection && isValidAccountSection && isValidPersonalInfoSection;
        }

        public void NotifyPropChanged(string propName = "CustomerToAdd")
        {
            OnPropertyChanged(propName);
        }

        private async void AddValidations()
        {
            _customerName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Customer Name can not be empty." });

            _customerCountry.Validations.Add(new IsComboboxNotSelected { ValidationMessage = "Country can not be empty." });
            _customerAddr1.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Address can not be empty." });
            _customerCity.Validations.Add(new IsComboboxNotSelected { ValidationMessage = "City can not be empty." });
            _customerState.Validations.Add(new IsComboboxNotSelected { ValidationMessage = "State can not be empty." });
            _customerZip.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Zip can not be empty." });

            _customerBillingAddr1.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Billing Address can not be empty." });
            _customerBillingCity.Validations.Add(new IsComboboxNotSelected { ValidationMessage = "Billing City can not be empty." });
            _customerBillingState.Validations.Add(new IsComboboxNotSelected { ValidationMessage = "Billing State can not be empty." });
            _customerBillingZip.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Billing Zip can not be empty." });

            _phoneNumber.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Phone Number can not be empty." });

        }

        #endregion
    }
}
