using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using DevExpress.XamarinForms.DataForm;
using DevExpress.XamarinForms.Editors;
using FocalPtMbl.MainMenu.ViewModels;
using FocalPoint.Data;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Collections;
using FocalPoint.Modules.FrontCounter.ViewModels;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Visum.Services.Mobile.Entities;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq.Expressions;
using FocalPoint.Modules.FrontCounter.Views;
using System.Collections.ObjectModel;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{
    public class CustomerEntryInfo : NotificationObject
    {
        private Customer customer;
        //public string Address1ToBill
        //{
        //    get => this.Address;
        //    private set
        //    {
        //        //If toggle = true
        //        if (CopyAddress)
        //        { 
        //        this.BillAddress = value;
        //        }
        //        OnPropertyChanged(nameof(BillAddress));
        //    }
        //}
        //public void AddressToBilling()
        //{
        //    this.BillAddress = this.BillAddress;
        //    this.BillCity = this.City;
        //    this.BillState = this.State;
        //    this.Zip = this.Zip;
        //}
        //    public string bindFirstName
        //    { 
        //        // get => this.customer.fname;
        //        private set
        //        {
        //            //If toggle = true
        //            if(true)
        //            this.FirstName = value;
        //           // OnPropertyChanged(nameof(FirstName));
        //        }
        //}
        const string leftColumnWidth = "0"; // was 40

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible =false, GroupName = "Names")]
        [DataFormItemPosition(RowOrder = 0)]
        [DataFormTextEditor(InplaceLabelText = "First name")]
        public string FirstName { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "Names")]
        [DataFormItemPosition(RowOrder = 1)]
        [DataFormTextEditor(InplaceLabelText = "Last name")]
        public string LastName { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false,   GroupName ="Names")]
        [DataFormItemPosition(RowOrder = 2)]
        [DataFormTextEditor(InplaceLabelText = "Company name")]
        public string CompanyName { get; set; }

        [DataFormDisplayOptions(LabelWidth = "250", IsLabelVisible = true, GroupName = "Address", LabelText = "Copy Address to Billing Address", LabelPosition = DataFormLabelPosition.Top)]
        [DataFormItemPosition(RowOrder = 0)]
        [DataFormSwitchEditor()]
        public bool CopyAddress { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "Address")]
        [DataFormItemPosition(RowOrder = 1)]
        [DataFormTextEditor(InplaceLabelText = "Address*")]
        [Required(ErrorMessage = "Address cannot be empty")]
        public string Address { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "Address")]
        [DataFormItemPosition(RowOrder = 2)]
        [DataFormTextEditor(InplaceLabelText = "Address cont.")]
        public string Address1cont { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "Address")]
        [DataFormItemPosition(RowOrder = 3)]
        [DataFormTextEditor(InplaceLabelText = "City*")]
        [Required(ErrorMessage = "City cannot be empty")]
        public string City { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, EditorWidth = "0.65*", IsLabelVisible = false, LabelText = "", GroupName = "Address")]
        [DataFormItemPosition(RowOrder = 4, ItemOrderInRow = 0)]
        [DataFormComboBoxEditor(InplaceLabelText = "State")]
        [Required(ErrorMessage = "State cannot be empty")]
        public string State { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, EditorWidth = "0.35*", EditorMaxWidth = 150, IsLabelVisible = false, GroupName = "Address")]
        [DataFormItemPosition(RowOrder = 4, ItemOrderInRow = 1)]
        [DataFormNumericEditor(InplaceLabelText = "Zip*")]
       // [RegularExpression(@"(^\d{5}$)|(^\d{5}-\d{4}$)", ErrorMessage = "Invalid zip-code")]
        public string Zip { get; set; }
        
        private string billAddress;
        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "Billing Address")]
        [DataFormItemPosition(RowOrder = 0)]
        [DataFormTextEditor(InplaceLabelText = "Billing Address*")]
        //[Required(ErrorMessage = "Address cannot be empty")]
        public string BillAddress { 
            get => billAddress;
            set => SetProperty(ref billAddress,value); 
        }
        private string billAddress1cont;
        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "Billing Address")]
        [DataFormItemPosition(RowOrder = 1)]
        [DataFormTextEditor(InplaceLabelText = "Billing Address cont.")]
        public string BillAddress1cont
        {
            get => billAddress1cont;
            set
            {
                SetProperty(ref billAddress1cont, value);
                OnPropertyChanged("BillAddress1cont");
            }
        }
        private string billCity;
        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "Billing Address")]
        [DataFormItemPosition(RowOrder = 2)]
        [DataFormTextEditor(InplaceLabelText = "Billing City*")]
        //[Required(ErrorMessage = "City cannot be empty")]
        public string BillCity {
            get => billCity;
            set => SetProperty(ref billCity, value);
        }
        private string billState;
        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, EditorWidth = "0.65*", IsLabelVisible = false, GroupName = "Billing Address")]
        [DataFormItemPosition(RowOrder = 3, ItemOrderInRow = 0)]
        [DataFormComboBoxEditor(InplaceLabelText = "Billing State")]
        //[Required(ErrorMessage = "State cannot be empty")]
        public string BillState {
            get => billState;
            set => SetProperty(ref billState, value);
        }
        private string billZip;
        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, EditorWidth = "0.35*", EditorMaxWidth = 150, IsLabelVisible = false, GroupName = "Billing Address")]
        [DataFormItemPosition(RowOrder = 3, ItemOrderInRow = 1)]
        [DataFormNumericEditor(InplaceLabelText = "Billing Zip*")]
        //[RegularExpression(@"(^\d{5}$)|(^\d{5}-\d{4}$)", ErrorMessage = "Invalid zip-code")]
        public string BillZip {
            get => billZip;
            set => SetProperty(ref billZip, value);
        }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "Phones", EditorWidth = "0.65*")]
        [DataFormItemPosition(RowOrder = 0)]
        [DataFormMaskedEditor(Mask = "(000) 000-0000", InplaceLabelText = "Phone number", Keyboard = "Telephone")]
        [Required(ErrorMessage = "Number cannot be empty")]
        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "Phone number must be 10 numbers in length")]
        public string PhoneNumber { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "Phones", EditorWidth = "0.35*")]
        [DataFormItemPosition(RowOrder = 0)]
        [DataFormComboBoxEditor(InplaceLabelText = "Phone Type")]

        public string PhoneType { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "Phones", EditorWidth = "0.65*")]
        [DataFormItemPosition(RowOrder = 1)]
        [DataFormMaskedEditor(Mask = "(000) 000-0000", InplaceLabelText = "Phone number2", Keyboard = "Telephone")]
        //[Required(ErrorMessage = "Number cannot be empty")]
        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "Phone number must be 8 numbers length")]
        public string PhoneNumber2 { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "Phones", EditorWidth = "0.35*")]
        [DataFormItemPosition(RowOrder = 1)]
        [DataFormComboBoxEditor(InplaceLabelText = "Phone Type")]

        public string PhoneType2 { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "Phones", EditorWidth = "0.65*")]
        [DataFormItemPosition(RowOrder = 2)]
        [DataFormMaskedEditor(Mask = "(000) 000-0000", InplaceLabelText = "Phone number3", Keyboard = "Telephone")]
        //[Required(ErrorMessage = "Number cannot be empty")]
        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "Phone number must be 8 numbers length")]
        public string PhoneNumber3 { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "Phones", EditorWidth = "0.35*")]
        [DataFormItemPosition(RowOrder = 2)]
        [DataFormComboBoxEditor(InplaceLabelText = "Phone Type")]

        public string PhoneType3 { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth,  IsLabelVisible = false, GroupName ="Emails")]
        [DataFormItemPosition(RowOrder = 0)]
        [DataFormTextEditor(InplaceLabelText = "Email", Keyboard = "Email")]
        public string Email { get; set; }
        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "Emails")]
        [DataFormItemPosition(RowOrder = 1)]
        [DataFormTextEditor(InplaceLabelText = "Email2", Keyboard = "Email")]
        public string Email2 { get; set; }
        [DataFormDisplayOptions(LabelWidth = leftColumnWidth,  IsLabelVisible = false, GroupName = "Emails")]
        [DataFormItemPosition(RowOrder = 2)]
        [DataFormTextEditor(InplaceLabelText = "Email3", Keyboard = "Email")]
        public string Email3 { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "Misc")]
        [DataFormItemPosition(RowOrder = 0)]
        [DataFormTextEditor(InplaceLabelText = "Referred By")]

        public string Referredby { get; set; }

        //[DataFormDisplayOptions(LabelWidth = leftColumnWidth,  IsLabelVisible = false, GroupName = "Account /Tax Info")]
        //[DataFormItemPosition(RowOrder = 3)]
        //[DataFormTextEditor(InplaceLabelText = "Referred By", Keyboard = "Email")]

        //public string Status { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "Account /Tax Info")]
        [DataFormItemPosition(RowOrder = 0)]
        [DataFormComboBoxEditor(InplaceLabelText = "Type")]

        public string Type { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "Account /Tax Info")]
        [DataFormItemPosition(RowOrder = 1)]
        [DataFormTextEditor(InplaceLabelText = "Designation")]

        public string Designation { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth,  IsLabelVisible = false, GroupName = "Account /Tax Info")]
        [DataFormItemPosition(RowOrder = 2)]
        [DataFormComboBoxEditor(InplaceLabelText = "Terms")]

        public string Terms { get; set; }

        //[DataFormDisplayOptions(LabelWidth = leftColumnWidth,  IsLabelVisible = false, GroupName = "Account /Tax Info")]
        //[DataFormItemPosition(RowOrder = 14)]
        //[DataFormTextEditor(InplaceLabelText = "Referred By", Keyboard = "Email")]

        //public string OldAccount { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth,  IsLabelVisible = false, GroupName = "Account /Tax Info")]
        [DataFormItemPosition(RowOrder = 3)]
        [DataFormTextEditor(InplaceLabelText = "Exempt ID", Keyboard = "Email")]
        public string ExemptID { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth,  IsLabelVisible = false, GroupName = "Personal Info")]
        [DataFormItemPosition(RowOrder = 0)]
        [DataFormTextEditor(InplaceLabelText = "License", Keyboard = "Email")]

        public string License { get; set; }

        private DateTime? expiration;
        //[DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "Personal Info")]
        //[DataFormItemPosition(RowOrder = 1)]
        //[DataFormDateEditor(InplaceLabelText = "Expiration",MinDate =null,DisplayFormat =null )]

        public DateTime? Experation
        {
            get => expiration;
            set => SetProperty(ref expiration, value);
        }


        [DataFormDisplayOptions(LabelWidth = leftColumnWidth,  IsLabelVisible = false, GroupName = "Personal Info")]
        [DataFormItemPosition(RowOrder = 1)]
        [DataFormComboBoxEditor(InplaceLabelText = "State")]

        public string PIState { get; set; }

        private DateTime? dOB;
        //[DataFormDisplayOptions(LabelWidth = leftColumnWidth,  IsLabelVisible = false, GroupName = "Personal Info")]
        //[DataFormItemPosition(RowOrder = 2)]
        //[DataFormDateEditor(InplaceLabelText = "DOB")]

        public DateTime? DOB
        {
            get => dOB;
            set => SetProperty(ref dOB, value);
        }


        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "Personal Info")]
        [DataFormItemPosition(RowOrder = 2)]
        [DataFormNumericEditor(InplaceLabelText = "Social Security", Keyboard = "Numeric")]

        public string SocailSecurity { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "SMS")]
        [DataFormItemPosition(RowOrder = 0)]
        [DataFormMaskedEditor(Mask = "(000) 000-0000", InplaceLabelText = "Phone number", Keyboard = "Telephone")]
        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "Phone number must be 10 numbers in length")]

        public string SMSPhone { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "SMS")]
        [DataFormItemPosition(RowOrder = 1)]
        [DataFormComboBoxEditor(InplaceLabelText = "Order Open")]

        public string OrderOpen { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "SMS")]
        [DataFormItemPosition(RowOrder = 1)]
        [DataFormComboBoxEditor(InplaceLabelText = "Order Close")]

        public string OrderClose { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "SMS")]
        [DataFormItemPosition(RowOrder = 2)]
        [DataFormComboBoxEditor(InplaceLabelText = "Reservation On-Hold")]

        public string RsrvOnHold { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "SMS")]
        [DataFormItemPosition(RowOrder = 2)]
        [DataFormComboBoxEditor(InplaceLabelText = "WO Completion")]

        public string WOCompletion { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "SMS")]
        [DataFormItemPosition(RowOrder = 3)]
        [DataFormComboBoxEditor(InplaceLabelText = "WO Finished")]

        public string WOFinished { get; set; }

        //[DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "SMS")]
        //[DataFormItemPosition(RowOrder = 3)]
        //[DataFormCheckBoxEditor()]

        //public string PrintAlsoIfSent { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "SMS")]
        [DataFormItemPosition(RowOrder = 4)]
        [DataFormComboBoxEditor(InplaceLabelText = "Send Reminders")]

        public string SendReminders { get; set; }

        [DataFormDisplayOptions(LabelWidth = leftColumnWidth, IsLabelVisible = false, GroupName = "SMS")]
        [DataFormItemPosition(RowOrder = 4)]
        [DataFormComboBoxEditor(InplaceLabelText = "Remind Annual Event Anniv")]

        public string RemindAnnual { get; set; }

        //[DataFormDisplayOptions( LabelWidth = leftColumnWidth,  IsLabelVisible = false, GroupName = "Names")]
        //[DataFormItemPosition(RowOrder = 5)]
        //[DataFormAutoCompleteEditor(InplaceLabelText = "Open Order", LoadingProgressMode =LoadingProgressMode.Auto, NoSuggestionsText = "ListofStates")]

        //public string OpenOrder { get; set; }
        }

    }

public class ComboBoxDataProvider : IPickerSourceProvider
{
    public ComboBoxDataProvider()
    {
        //setHttpClient headers
        //get comboboxlists
        var httpClientCache = DependencyService.Resolve<FocalPoint.MainMenu.Services.IHttpClientCacheService>();
        this.clientHttp = httpClientCache.GetHttpClientAsync();
        states = new List<string>();
        //StatesAPI = new List<DisplayValueString>();
        states = GetStates();
    }
    HttpClient clientHttp;
    public HttpClient ClientHTTP
    {
        get { return clientHttp; }
    }
    private List<string> states;
    //public List<DisplayValueString> StatesAPI;
    CustomerSettings customerSettings = null;
    private List<string> GetStates()
    {

        Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "CustomerSettings/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
        try
        {
            var response = ClientHTTP.GetAsync(uri).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                customerSettings = JsonConvert.DeserializeObject<CustomerSettings>(content);
            }
            //    //get Country Code
            Uri uri2 = new Uri(string.Format(DataManager.Settings.ApiUri + "States/" + customerSettings.Defaults.CustomerCountry.ToString()));

            var response2 = ClientHTTP.GetAsync(uri2).GetAwaiter().GetResult();
            if (response2.IsSuccessStatusCode)
            {
                string content2 = response2.Content.ReadAsStringAsync().Result;
                List<DisplayValueString> statesAPI = JsonConvert.DeserializeObject<List<DisplayValueString>>(content2);
                foreach (var state in statesAPI)
                {
                    states.Add(state.Value);
                    // StatesAPI.Add(state);
                }
            }
            else
                states = new List<string>();
            return states;
        }
        catch (Exception ex)
        {
            return states = new List<string>();
        }
    }
    public List<string> GetComboBoxes(string propName)
    {
        List<string> displaystrings = new List<string>();
        if (customerSettings != null)
        {
            if (propName == "Type")
            {
                //customer status
               foreach( var type in customerSettings.CustomerCTypes)
                {
                    displaystrings.Add(type.Display);
                }
            }
            else if (propName == "Terms")
            {
                //customer status
                foreach (var type in customerSettings.CustomerTerms)
                {
                    displaystrings.Add(type.Display);
                }
            }
            else if (propName == "CustomerFounds")
            {
                //customer status
                foreach (var type in customerSettings.CustomerFounds)
                {
                    displaystrings.Add(type.Display);
                }
            }
            else if (propName == "CustomerTypes")
            {
                //customer status
                foreach (var type in customerSettings.CustomerTypes)
                {
                    displaystrings.Add(type.Display);
                }
            }
            else if (propName == "Status")
            {
                //customer status
                foreach (var type in customerSettings.CustomerStatus)
                {
                    displaystrings.Add(type.Display);
                }
            }
        }
        return displaystrings;
    }
    public IEnumerable GetSource(string propertyName)
    {
        if (propertyName == "Status")
        {
            return new List<string>() {
                    "Salaried",
                    "Commission",
                    "Terminated",
                    "On Leave"
                };
        }
        //attempt get call
        if (propertyName == "State" || propertyName == "BillState" || propertyName == "PIState")
        {
            return states;
                
        }
        if(propertyName == "Type" || propertyName == "Terms")
        {
            return GetComboBoxes(propertyName);
        }
        if (propertyName == "PhoneType" || propertyName == "PhoneType2" || propertyName == "PhoneType3")
        {
            return new List<string>() {
                    "Fax",
                    "Cell",
                    "Work",
                    "Home"
                };
        }
        if (propertyName == "OrderOpen" || propertyName == "OrderClose" || propertyName == "RsrvOnHold" || propertyName == "WOCompletion" || propertyName == "WOFinished" || propertyName == "SendReminders" || propertyName == "RemindAnnual")
        {
            return new List<string>()
                {
                    "Email",
                    "SMS",
                    "Both"
                };
        }

        return null;
        }
    }
    public class CustomerLupViewModel : ThemeBaseViewModel
{
    HttpClient clientHttp;
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
    public CustomerEntryInfo model;

    public Command DownloadCommand { get; }
    public ObservableCollection<string> Greetings { get; set; }
    public CustomerEntryInfo Model {
        get => model;
        set
        {
            if (model != value)
            {
                model = value;
                OnPropertyChanged("Model");
            }
        }
    }
    private Customer newCustomer;
        public Customer NewCustomer { 
            get => newCustomer;
            set
            {
                if (newCustomer != value)
                {
                    newCustomer = value;
                    OnPropertyChanged("NewCustomer");
                }
            }
        }

    public CustomerLupViewModel()
        {
        DownloadCommand = new Command(async () => await DownloadAsync());
        Model = new CustomerEntryInfo();
            NewCustomer = new Customer();
        //add store
        var httpClientCache = DependencyService.Resolve<FocalPoint.MainMenu.Services.IHttpClientCacheService>();
        this.clientHttp = httpClientCache.GetHttpClientAsync();
        //API call to get customer/storesettings / Anytime we have list to populate the fields...
        custSettings = GetCustomerSettings();
        MessagingCenter.Subscribe<CustomerLupView, string>(this, "Hi", (sender, arg) =>
        {
            Greetings.Add("Hi " + arg);
        });
        //newCustomer.CustomerTerms = customerSettings. CustomerTerms;
    }
    public CustomerSettings custSettings = new CustomerSettings();

    async Task DownloadAsync()
    {
        await Task.Run(() => Download());
    }

    int Download()
    {
        Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "CustomerSettings/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
        long ticks = 100;
        ClientHTTP.Timeout = new TimeSpan(ticks);
        try
        {
            var response = ClientHTTP.GetAsync(uri).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                return 200;
            }
            return ((int)response.StatusCode);
        }catch(Exception ex)
        {
            return 404;
        }
       // ...
    }


        internal void SetCustomerData ()
        {
        //check
        //  NewCustomer.BillCityStateZip = Model.BillCity + Model.BillState + Model.BillZip; Ignore this
        //  NewCustomer.CityStateZip = Model.City + Model.State + Model.Zip;
        NewCustomer.CustomerAddr1 = Model.Address + Model.Address1cont;
        NewCustomer.CustomerARBal = 0;
        NewCustomer.CustomerBDte = Model.DOB;
        NewCustomer.CustomerBillAddr1 = Model.BillAddress + Model.BillAddress1cont;
        NewCustomer.CustomerBillCity = Model.BillCity;
        NewCustomer.CustomerBillState = Model.BillState;
        NewCustomer.CustomerBillZip = Model.BillZip.ToString();
        NewCustomer.CustomerCity = Model.City;
        NewCustomer.CustomerContact = "";
        // NewCustomer.CustomerCountry = ; Default
        //NewCustomer.CustomerCType = ; Default
        NewCustomer.CustomerDLEDte =  Model.Experation;
        NewCustomer.CustomerDrLic = Model.License;
        NewCustomer.CustomerDrLicSt = Model.PIState;
        //NewCustomer.CustomerDW = ""; Default
        NewCustomer.CustomerEmail = Model.Email;
        NewCustomer.CustomerEMail2 = Model.Email2;
        NewCustomer.CustomerEMail3 = Model.Email3;
        NewCustomer.CustomerFName = Model.FirstName;
       // NewCustomer.CustomerFoundNo = "";
       // NewCustomer.CustomerLimit = "";
        NewCustomer.CustomerLName = Model.LastName;
        NewCustomer.CustomerName = Model.CompanyName;// check settings to determin f or l name first
       // NewCustomer.CustomerNo = ""; // get new custno
        NewCustomer.CustomerNotes = ""; // Make notes section
       // NewCustomer.CustomerNotes_HTML = "";
        NewCustomer.CustomerPhone = Model.PhoneNumber;
        NewCustomer.CustomerPhone2 = Model.PhoneNumber2;
        NewCustomer.CustomerPhone3 = Model.PhoneNumber3;
       // NewCustomer2.CustomerPhoneType = "c"; //Model.PhoneType; // ???
       // NewCustomer2.CustomerPhoneType2 = Model.PhoneType2; // ???
       // NewCustomer2.CustomerPhoneType3 = Model.PhoneType3; // ???
        NewCustomer.CustomerSMSNumber = Model.SMSPhone; // get sms section
        NewCustomer.CustomerSS = Model.SocailSecurity;  // ???
        NewCustomer.CustomerState = Model.State;
       // NewCustomer.CustomerStatus = ""; //new status
       // NewCustomer.CustomerStatus_Display = "";
        NewCustomer.CustomerStore = 1; // Current Store
        NewCustomer.CustomerTaxNo = Model.ExemptID;
  //FIX      NewCustomer.CustomerTerms = Model.Terms;// get terms?
        //NewCustomer2.CustomerType = Model.Type;
        NewCustomer.CustomerType_Display = "";
        NewCustomer.CustomerZip = Model.Zip.ToString();

    }
        internal CustomerSettings GetCustomerSettings()
        {
        try
        {
            CustomerSettings customerSettings = null;

            Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "CustomerSettings/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
            //long ticks = 5000;
            var response = ClientHTTP.GetAsync(uri).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                customerSettings = JsonConvert.DeserializeObject<CustomerSettings>(content);
                //SetDefaults
                NewCustomer.CustomerCountry = customerSettings.Defaults.CustomerCountry;
                NewCustomer.CustomerDW = customerSettings.Defaults.CustomerDW;
                NewCustomer.CustomerPhoneType = customerSettings.Defaults.CustomerPhoneType;
                NewCustomer.CustomerPhoneType2 = customerSettings.Defaults.CustomerPhoneType2;
                NewCustomer.CustomerPhoneType3 = customerSettings.Defaults.CustomerPhoneType3;
                NewCustomer.CustomerStatus = customerSettings.Defaults.CustomerStatus;
                NewCustomer.CustomerStore = customerSettings.Defaults.CustomerStore;
                NewCustomer.CustomerTerms = customerSettings.Defaults.CustomerTerms;
                NewCustomer.CustomerType = customerSettings.Defaults.CustomerType;
                
                //Model.
                //NewCustomer.CustomerTerms
                // Model.
            }
            return customerSettings;
            // ClientHTTP.
        }
        catch(Exception ex)
        {
            return new CustomerSettings();
        }
        }
        bool isVertical = true;
        public void AddressSameAsBillAdress(bool changeBillAddress, DataFormItem dataFormItem)
        {
            if(changeBillAddress)
            {

            if (dataFormItem.FieldName == "BillAddress")
            {
                Model.BillAddress = Model.Address;
                dataFormItem.LabelText = Model.Address;
                
                //dataFormItem.PropertyChanged;
            }
            if (dataFormItem.FieldName == "BillCity")
            {
                Model.BillCity = Model.City;
                dataFormItem.LabelText = Model.City;
            }
            if (dataFormItem.FieldName == "BillState")
            {
                Model.BillState = Model.State;
                dataFormItem.LabelText = Model.State;
            }
            if (dataFormItem.FieldName == "BillZip")
            {
                Model.BillZip = Model.Zip;
                dataFormItem.LabelText = Model.Zip;
            }

            
            }
        }

        public void Rotate(DataFormView dataForm, bool newIsVertical)
        {
            //if (newIsVertical != isVertical)
            //{
            //    if (dataForm.Items != null)
            //    {
            //        isVertical = newIsVertical;
            //        foreach (KeyValuePair<string, bool> fieldName in fieldNamesToReorder)
            //        {
            //            DataFormItem item = dataForm.Items.FirstOrDefault(i => i.FieldName == fieldName.Key);
            //            int modifier = newIsVertical ? 1 : -1;
            //            if (item != null)
            //            {
            //                item.RowOrder += modifier;
            //                if (fieldName.Value)
            //                    item.IsLabelVisible = newIsVertical;
            //            }
            //        }
            //    }
            //}
        }

    internal void CheckModel()
    {
        Model.Address = "...";
    }

    internal void UpdateModel(string propertyName, object newValue)
    {
        switch (propertyName)
        {
            case "Address":
                Model.Address = newValue.ToString();
                break;
            case "Address1cont":
                Model.Address1cont = newValue.ToString();
                break;
            case "BillAddress":
                Model.BillAddress = newValue.ToString();
                break;
            case "BillAddress1cont":
                Model.BillAddress1cont = newValue.ToString();
                break;
            case "BillCity":
                Model.BillCity = newValue.ToString();
                break;
            case "BillState":
                Model.BillState = newValue.ToString();
                break;
            case "BillZip":
                Model.BillZip = newValue.ToString();
                break;
            case "City":
                Model.City = newValue.ToString();
                break;
            case "CompanyName":
                Model.CompanyName = newValue.ToString();
                break;
            case "Designation":
                Model.Designation = newValue.ToString();
                break;
            case "DOB":
                Model.DOB = (DateTime?)newValue;
                break;
            case "Email":
                Model.Email = newValue.ToString();
                break;
            case "Email2":
                Model.Email2 = newValue.ToString();
                break;
            case "Email3":
                Model.Email3 = newValue.ToString();
                break;
            case "ExemptID":
                Model.ExemptID = newValue.ToString();
                break;
            case "Experation":
                Model.Experation = (DateTime?)newValue;
                break;
            case "FirstName":
                Model.FirstName = newValue.ToString();
                break;
            case "LastName":
                Model.LastName = newValue.ToString();
                break;
            case "License":
                Model.License = newValue.ToString();
                break;
            //case "OldAccount":
            //    Model.OldAccount = newValue.ToString();
            //    break;
            //case "OpenOrder":
            //    Model.OpenOrder = newValue.ToString();
            //    break;
            case "OrderClose":
                Model.OrderClose = newValue.ToString();
                break;
            case "OrderOpen":
                Model.OrderOpen = newValue.ToString();
                break;
            case "PhoneNumber":
                Model.PhoneNumber = newValue.ToString();
                break;
            case "PhoneType":
                Model.PhoneType = newValue.ToString();
                break;
            case "PhoneNumber2":
                Model.PhoneNumber2 = newValue.ToString();
                break;
            case "PhoneType2":
                Model.PhoneType2 = newValue.ToString();
                break;
            case "PhoneNumber3":
                Model.PhoneNumber3 = newValue.ToString();
                break;
            case "PhoneType3":
                Model.PhoneType3 = newValue.ToString();
                break;
            case "PIState":
                Model.PIState = newValue.ToString();
                break;
            //case "PrintAlsoIfSent":
            //    Model.PrintAlsoIfSent = newValue.ToString();
            //    break;
            case "Referredby":
                Model.Referredby = newValue.ToString();
                break;
            case "RemindAnnual":
                Model.RemindAnnual = newValue.ToString();
                break;
            case "RsrvOnHold":
                Model.RsrvOnHold = newValue.ToString();
                break;
            case "SendReminders":
                Model.SendReminders = newValue.ToString();
                break;
            case "SMSPhone":
                Model.SMSPhone = newValue.ToString();
                break;
            case "SocailSecurity":
                Model.SocailSecurity = newValue.ToString();
                break;
            case "State":
                Model.State = newValue.ToString();
                break;
            //case "Status":
            //    Model.Status = newValue.ToString();
            //    break;
            case "Terms":
                Model.Terms = newValue.ToString();
                break;
            case "Type":
                Model.Type = newValue.ToString();
                break;
            case "WOCompletion":
                Model.WOCompletion = newValue.ToString();
                break;
            case "WOFinished":
                Model.WOFinished = newValue.ToString();
                break;
            case "Zip":
                Model.Zip = newValue.ToString();
                break;

            default:
                break;
        }
        if (Model.CopyAddress)
        {
            Model.BillAddress = Model.Address;
            Model.BillCity = Model.City;
            Model.BillState = Model.State;
            Model.BillZip = Model.Zip;

        }

    }

    internal bool SendNewCustomer()
    {
        string testURIBase = DataManager.Settings.ApiUri;
        try {
            //this needs to be adjusted based on the store settings
            if(NewCustomer.CustomerName == null)
                NewCustomer.CustomerName = NewCustomer.CustomerLName + ", " + NewCustomer.CustomerFName ;
            if (newCustomer.CustomerPhoneType == null)
                newCustomer.CustomerPhoneType = custSettings.Defaults.CustomerPhoneType; ;

        Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "Customer/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                                                                                                             // Customer Customer = NewCustomer;
        //Customer NewCustomer = NewCustomer2;
        string JsonCustomer = JsonConvert.SerializeObject( new { NewCustomer });
        string jsonparse = JToken.Parse(JsonCustomer).ToString();
        var stringContent = new StringContent(
                                JsonConvert.SerializeObject(new { NewCustomer }),
                                  Encoding.UTF8,
                                  "application/json");

        var response = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
        if (response.IsSuccessStatusCode)
        {
            string content = response.Content.ReadAsStringAsync().Result;
            JsonConvert.DeserializeObject<Customer>(content);
            //customersCntAndList = JsonConvert.DeserializeObject<List<Data.DataLayer.AvailabilityMerch>>(content);
            return true;
        }
        else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            // maybe just retry once obtaining new token
            var test = "";
        }
        else
        {
            // return to view with error Get new token??
            var test = "";        
        }

           // byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //var myContent = JsonConvert.SerializeObject(new { NewCustomer });
            //var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            //var byteContent = new ByteArrayContent(buffer);
            //var response2 = ClientHTTP.PostAsync(uri, byteContent).GetAwaiter().GetResult();
            //if (response2.IsSuccessStatusCode)
            //{
            //    string content = response2.Content.ReadAsStringAsync().Result;
            //    JsonConvert.DeserializeObject<FocalPoint.Data.DataLayer.Customer>(content);
            //    //customersCntAndList = JsonConvert.DeserializeObject<List<Data.DataLayer.AvailabilityMerch>>(content);
            //    return true;
            //}
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    internal bool CheckNumbers()
    {
        string CustomerNo = "0";
        string PhoneNumber = Model.PhoneNumber;
        try
        {
            Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "Customer/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));

            var stringContent = new StringContent(
                                      JsonConvert.SerializeObject(new { CustomerNo, PhoneNumber }),
                                      Encoding.UTF8,
                                      "application/json");
            //var response =  viewModel.ClientHTTP.PostAsync(uri, stringContent).Result;
            var response = clientHttp.PostAsync(uri, stringContent).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    internal bool CheckNames()
    {
        string CustomerNo = "0";
        string FirstName = Model.FirstName;
        string LastName = Model.LastName;
        try
        {
            Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "Customer/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));

            var stringContent = new StringContent(
                                      JsonConvert.SerializeObject(new { CustomerNo, FirstName, LastName }),
                                      Encoding.UTF8,
                                      "application/json");
            //var response =  viewModel.ClientHTTP.PostAsync(uri, stringContent).Result;
            var response = clientHttp.PostAsync(uri, stringContent).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
