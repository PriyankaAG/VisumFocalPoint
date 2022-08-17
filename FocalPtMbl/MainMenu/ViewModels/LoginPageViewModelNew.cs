using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using FocalPoint.MainMenu.Services;
using FocalPoint.Utils;
using FocalPoint.Validations;
using FocalPoint.Validations.Rules;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using System.Linq;
using FocalPoint.Data;
using FocalPoint.Data.DataModel;

namespace FocalPoint.MainMenu.ViewModels
{
    public class LoginPageViewModelNew : ThemeBaseViewModel
    {
        #region Fields
        HttpClient clientHttp = new HttpClient();
        Dictionary<string, Company> storeDict = new Dictionary<string, Company>();
        Dictionary<string, Terminal> terminalDict = new Dictionary<string, Terminal>();

        public ISettingsComponent SettingsComponent;
        #endregion

        #region Properties
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        private string user = "";
        public string User
        {
            get { return user; }
            set
            {
                if (user != value)
                {
                    user = value;
                    OnPropertyChanged(nameof(User));
                }
            }
        }
        public string Token { get; set; }

        private string _baseUrl;
        public string BaseURL
        {
            get
            {
                _baseUrl = HostName.Value + ":" + PortNumber.Value;
                if (!_baseUrl.Contains("https://"))
                    _baseUrl = "https://" + _baseUrl;
                return _baseUrl;
            }
        }

        internal string storeLoginNo = "0";
        public string StoreLoginNo
        {
            get { return storeLoginNo; }
            set
            {
                if (storeLoginNo != value)
                {
                    storeLoginNo = value;
                    OnPropertyChanged("StoreLoginNo");
                }
            }

        }

        internal string terminalNo = "0";
        public string TerminalNo
        {
            get { return terminalNo; }
            set
            {
                if (terminalNo != value)
                {
                    terminalNo = value;
                    OnPropertyChanged("TerminalNo");
                }
            }

        }

        private ValidatableObject<string> _userName;
        public ValidatableObject<string> UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        private ValidatableObject<string> _portNumber;
        public ValidatableObject<string> PortNumber
        {
            get
            {
                return _portNumber;
            }
            set
            {
                _portNumber = value;
                OnPropertyChanged(nameof(PortNumber));
            }
        }

        private ValidatableObject<string> _hostName;
        public ValidatableObject<string> HostName
        {
            get
            {
                return _hostName;
            }
            set
            {
                _hostName = value;
                OnPropertyChanged(nameof(HostName));
            }
        }

        private ValidatableObject<string> _userpassword;
        public ValidatableObject<string> UserPassword
        {
            get
            {
                return _userpassword;
            }
            set
            {
                _userpassword = value;
                OnPropertyChanged(nameof(UserPassword));
            }
        }

        public bool IsSignedIn { get; set; }
        #endregion

        #region Commands
        public ICommand ValidateUserNameCommand => new Command(() => ValidateUserName());
        public ICommand ValidatePasswordCommand => new Command(() => ValidatePassword());
        public ICommand ValidatePortNumberCommand => new Command(() => ValidatePortNumber());
        public ICommand ValidateHostNameCommand => new Command(() => ValidateHostName());
        private string getFingerPrint() => DependencyService.Resolve<IDeviceInfo>().DeviceId;
        public bool ValidateLogin() => Validate();

        #endregion

        public LoginPageViewModelNew()
        {
            clientHttp = PreparedClient();
            SettingsComponent = new SettingsComponent();
            _userName = new ValidatableObject<string>();
            _userpassword = new ValidatableObject<string>();
            _hostName = new ValidatableObject<string>();
            _portNumber = new ValidatableObject<string>();

            CheckForSettings();
            AddValidations();
        }

        private HttpClient PreparedClient()
        {
            HttpClientHandler handler = new HttpClientHandler();
            //not sure about this one, but I think it should work to allow all certificates:
            handler.ServerCertificateCustomValidationCallback += (sender, cert, chaun, ssPolicyError) =>
            {
                return true;
            };
            HttpClient client = new HttpClient(handler);
            return client;
        }

        private async void CheckForSettings()
        {
            HostName.Value = await SettingsComponent.GetSecure(Utils.Utils.HOSTKEY);
            PortNumber.Value = await SettingsComponent.GetSecure(Utils.Utils.PORTKEY) ?? "56883";
        }

        private void AddValidations()
        {
            _userName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Login or Password Incorrect" });
            _portNumber.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Port incorrect or Unavailable" });
            _hostName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Host incorrect or Unavailable" });
            _userpassword.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Login or Password Incorrect" });
        }

        private bool Validate()
        {
            bool isValidUser = ValidateUserName();
            bool isValidPassword = ValidatePassword();
            bool isValidHostName = ValidateHostName();
            bool isValidPortNumber = ValidatePortNumber();

            return isValidUser && isValidPassword && isValidHostName && isValidPortNumber;
        }

        private bool ValidateUserName()
        {
            return _userName.Validate();
        }

        private bool ValidatePassword()
        {
            return _userpassword.Validate();
        }

        private bool ValidatePortNumber()
        {
            return _portNumber.Validate();
        }

        private bool ValidateHostName()
        {
            return _hostName.Validate();
        }

        public int AttemptLogin()
        {
            try
            {
                string Username = UserName.Value;
                string Password = UserPassword.Value; //"+Eg/bz+kAL5rp1moMUdS7B5o1MQZPNxbvi3bdu05huI=";
                Password = DependencyService.Get<ICrypt>().Encrypt("VISLLc0404", Password);
                //UserPassword = "E0H8L8g/9Uw8OxYe44ZgvsSa36GfroU49AEtLLiT3S4=";

                Uri uri = new Uri(string.Format(BaseURL + "/Mobile/Login"));
                var stringContent = new StringContent(
                                      JsonConvert.SerializeObject(new { Username, Password }),
                                      Encoding.UTF8,
                                      "application/json");

                var response = ClientHTTP.PostAsync(uri, stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    var content2 = response.Content.ReadAsStringAsync().Result.ToString();
                    var token = JsonConvert.DeserializeObject<Guid>(content2);
                    if (token == Guid.Empty)
                    {
                        return 3;
                    }
                    else
                    {
                        User = token.ToString();
                        return 5;
                    }
                }
                else
                    return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        internal int CheckLicenses()
        {

            Uri uri3 = new Uri(string.Format(BaseURL + "/Mobile/V1/ConnectionLimit"));
            //string Username = this.Model.Login;
            string FingerPrint = getFingerPrint();
            short Type = Utils.Utils.GetDeviceType();
            string Phone = "";

            var stringContent3 = new StringContent(JsonConvert.SerializeObject(new { FingerPrint, Type, Phone }), Encoding.UTF8, "application/json");
            if (ClientHTTP.DefaultRequestHeaders.Contains("User"))
                ClientHTTP.DefaultRequestHeaders.Remove("User");
            ClientHTTP.DefaultRequestHeaders.Add("User", User);

            try
            {
                var response = ClientHTTP.PostAsync(uri3, stringContent3).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result.ToString();
                    var token = JsonConvert.DeserializeObject<Guid>(content);
                    if (token == Guid.Empty)
                    {
                        return 0;
                    }
                    else
                    {
                        Token = token.ToString();
                        if (ClientHTTP.DefaultRequestHeaders.Contains("Token"))
                            ClientHTTP.DefaultRequestHeaders.Remove("Token");
                        ClientHTTP.DefaultRequestHeaders.Add("Token", Token);
                        return 1;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        internal string[] GetStoresArray()
        {
            List<string> CurrentStores = new List<string>();
            Uri uriStores = new Uri(string.Format(BaseURL + "/Mobile/V1/LoginStores"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
            var responseDR = ClientHTTP.GetAsync(uriStores).GetAwaiter().GetResult();
            if (responseDR.IsSuccessStatusCode)
            {
                storeDict = new Dictionary<string, Company>();
                var content = responseDR.Content.ReadAsStringAsync().Result;
                var Stores = JsonConvert.DeserializeObject<List<Company>>(content);
                foreach (var store in Stores)
                {
                    CurrentStores.Add(store.DisplayName);
                    storeDict.Add(store.DisplayName, store);
                }
                List<Store> stores = new List<Store>();
                foreach (Company company in storeDict.Values)
                {
                    stores.Add(new Store()
                    {
                        CmpNo = company.CmpNo,
                        CmpName = company.CmpName,
                        DisplayName = company.DisplayName
                    });
                }
                DataManager.SaveStores(stores);
            }

            return CurrentStores.ToArray();
        }
        internal string[] GetTerminalArray()
        {
            // if store was selected but no terminal remove the headder and readd the new one
            if (ClientHTTP.DefaultRequestHeaders.Contains("StoreNo"))
                ClientHTTP.DefaultRequestHeaders.Remove("StoreNo");
            ClientHTTP.DefaultRequestHeaders.Add("StoreNo", StoreLoginNo);

            List<string> CurrentTerminals = new List<string>();
            Uri uriStores = new Uri(string.Format(BaseURL + "/Mobile/V1/Terminals"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
            var responseDR = ClientHTTP.GetAsync(uriStores).GetAwaiter().GetResult();
            if (responseDR.IsSuccessStatusCode)
            {
                terminalDict = new Dictionary<string, Terminal>();
                var content = responseDR.Content.ReadAsStringAsync().Result;
                var Terminals = JsonConvert.DeserializeObject<List<Terminal>>(content);
                foreach (var terminal in Terminals)
                {
                    CurrentTerminals.Add(terminal.TerminalID);
                    terminalDict.Add(terminal.TerminalID, terminal);
                }

            }
            return CurrentTerminals.ToArray();
        }
        internal string GetStoreFromArray(string loginStore)
        {
            return storeDict.ContainsKey(loginStore) ? storeDict[loginStore].CmpNo.ToString() : "";
        }
        internal string GetTerminalFromArray(string loginTerminal)
        {
            return terminalDict.ContainsKey(loginTerminal) ? terminalDict[loginTerminal].TerminalNo.ToString() : "";
        }
        internal bool LoginSecurity()
        {
            Uri uriSec = new Uri(string.Format(BaseURL + "/Mobile/V1/LoginSecurity"));
            List<FocalPoint.Data.DataModel.Security> secAreas = new List<FocalPoint.Data.DataModel.Security>();

            var response = ClientHTTP.GetAsync(uriSec).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result.ToString();
                var allowedAreas = JsonConvert.DeserializeObject<List<FocalPoint.Data.DataModel.Security>>(content);
                secAreas.AddRange(from area in allowedAreas
                                  select area);

                if (!ClientHTTP.DefaultRequestHeaders.Contains("StoreNo"))
                    ClientHTTP.DefaultRequestHeaders.Add("StoreNo", StoreLoginNo);
                if (!ClientHTTP.DefaultRequestHeaders.Contains("TerminalNo"))
                    ClientHTTP.DefaultRequestHeaders.Add("TerminalNo", TerminalNo);

                DataManager.Settings.UserName = UserName.Value;
                DataManager.Settings.ApiUri = BaseURL + "/Mobile/V1/";
                DataManager.Settings.HomeStore = int.Parse(StoreLoginNo);
                DataManager.Settings.Terminal = int.Parse(TerminalNo);
                DataManager.Settings.UserToken = Token;
                DataManager.Settings.User = User;
                DataManager.Settings.IsSignedIn = IsSignedIn;
                // NEEDS SQLite_Android.cs Implementation  DataManager.SaveSettings();
                DataManager.SaveSettings();

                DataManager.ResetSecurityTable();

                if (secAreas.Any())
                {
                    var NoRowsAdded = DataManager.InsertAllSecurities(secAreas);
                }

                var httpClientCache = DependencyService.Resolve<IHttpClientCacheService>();
                httpClientCache.BaseUrl = (BaseURL + "/Mobile/V1/");
                httpClientCache.Store = StoreLoginNo;
                httpClientCache.Terminal = TerminalNo;
                httpClientCache.Token = Token;
                httpClientCache.User = User;
                httpClientCache.AddClient(BaseURL, StoreLoginNo, TerminalNo, Token, User, ClientHTTP);
                this.clientHttp = httpClientCache.GetHttpClientAsync();
                return true;
            }
            return false;
        }
        internal void SetSecures()
        {
            SettingsComponent.SetSecure(Utils.Utils.HOSTKEY, HostName.Value);
            SettingsComponent.SetSecure(Utils.Utils.PORTKEY, PortNumber.Value);
        }
    }
}
