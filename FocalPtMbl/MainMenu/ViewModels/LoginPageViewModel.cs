using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using DevExpress.XamarinForms.DataForm;
using FocalPoint.Data;
using FocalPoint.Data.API;
using FocalPoint.MainMenu.Services;
using FocalPoint.Utils;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using Visum.Services.Mobile.Entities;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace FocalPoint.MainMenu.ViewModels
{
    public class LoginInfo : NotificationObject
    {
        private string connectionPort = "56883";
        const string leftColumnWidth = "0"; //was 40

        //[DataFormDisplayOptions(LabelWidth = leftColumnWidth, LabelIcon = "")]
        [DataFormDisplayOptions(IsLabelVisible = false)]
        [DataFormItemPosition(RowOrder = 0)]
        [DataFormTextEditor(InplaceLabelText = "Host", Keyboard = "Email")]
        [Required(ErrorMessage = "Host incorrect or Unavailable")]
        public string ConnectionURL { get; set; }

        [DataFormDisplayOptions(IsLabelVisible = false)]
        [DataFormItemPosition(RowOrder = 1)]
        [DataFormTextEditor(InplaceLabelText = "Port", Keyboard = "Numeric")]
        [Required(ErrorMessage = "Port incorrect or Unavailable")]
        public string ConnectionPort
        {
            get => connectionPort;
            set => SetProperty(ref connectionPort, value);
        }

        [DataFormDisplayOptions(IsLabelVisible = false)]
        [DataFormItemPosition(RowOrder = 2)]
        [DataFormTextEditor(InplaceLabelText = "Username")]
        [Required(ErrorMessage = "Login or Password Incorrect")]
        public string Login { get; set; }

        [DataFormDisplayOptions(IsLabelVisible = false)]
        [DataFormItemPosition(RowOrder = 3)]
        [DataFormPasswordEditor(InplaceLabelText = "Password"),]
        [Required(ErrorMessage = "Login or Password Incorrect")]
        public string Password { get; set; }

        //string IDataErrorInfo.Error => String.Empty;

        //string IDataErrorInfo.this[string columnName]
        //{
        //    get { return String.Empty; }
        //}
    }
    public class LoginPageViewModel : ThemeBaseViewModel
    {
        ICommand buttonCommand = null;
        HttpClient clientHttp = new HttpClient();
        public ISettingsComponent SettingsComponent;

        public LoginInfo Model { get; set; }

        public LoginPageViewModel()
        {
            Model = new LoginInfo();
            clientHttp = PreparedClient();
            //secureClientHttp = new HttpClient();
            buttonCommand = new ValidationCommand(this);
            SettingsComponent = new SettingsComponent();
            CheckForSettings();
        }
        /// <summary>
        /// Checks whether the user had logged in previously and has a HOST and a PORT saved 
        /// onto his device.
        /// </summary>
        private async void CheckForSettings()
        {
            Model.ConnectionURL = await SettingsComponent.GetSecure(Ultils.HOSTKEY);
            Model.ConnectionPort = await SettingsComponent.GetSecure(Ultils.PORTKEY);

            if (string.IsNullOrEmpty(Model.ConnectionPort))
                Model.ConnectionPort = "56883";
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
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        //public HttpClient SecureClientHttp
        //{
        //    get { return secureClientHttp; }
        //}
        public ICommand ButtonCommand
        {
            get { return buttonCommand; }
            set
            {
                if (buttonCommand != value)
                {
                    buttonCommand = value;
                    OnPropertyChanged("ButtonCommand");
                }
            }
        }

        Dictionary<string, bool> fieldNamesToReorder = new Dictionary<string, bool>() {
            { nameof(LoginInfo.ConnectionURL), true },
            { nameof(LoginInfo.Login), true },
            { nameof(LoginInfo.Password), true },
        };

        bool isVertical = true;
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
        public void Rotate(DataFormView dataForm, bool newIsVertical)
        {
            if (newIsVertical != isVertical)
            {
                if (dataForm.Items != null)
                {
                    isVertical = newIsVertical;
                    foreach (KeyValuePair<string, bool> fieldName in fieldNamesToReorder)
                    {
                        DataFormItem item = dataForm.Items.FirstOrDefault(i => i.FieldName == fieldName.Key);
                        int modifier = newIsVertical ? 1 : -1;
                        if (item != null)
                        {
                            item.RowOrder += modifier;
                            if (fieldName.Value)
                                item.IsLabelVisible = newIsVertical;
                        }
                    }
                }
            }
        }
        private string curToken = "";
        public string CurToken
        {
            get { return curToken; }
            set
            {
                if (curToken != value)
                {
                    curToken = value;
                    OnPropertyChanged("CurToken");
                }
            }
        }
        public int AttemptLogin(DataFormView dataForm)
        {
            try
            {

                if (dataForm.Items != null)
                {
                    string baseURL = this.Model.ConnectionURL + ":" + this.Model.ConnectionPort;
                    if (!baseURL.Contains("https://"))
                        baseURL = "https://" + baseURL;

                    string Username = this.Model.Login;
                    string Password = this.Model.Password; //"+Eg/bz+kAL5rp1moMUdS7B5o1MQZPNxbvi3bdu05huI=";
                    Password = DependencyService.Get<ICrypt>().Encrypt("VISLLc0404", Password);

                    Uri uri = new Uri(string.Format(baseURL + "/Mobile/Compatible"));
                    Uri uri2 = new Uri(string.Format(baseURL + "/Mobile/Login"));
                    var stringContent = new StringContent(
                                                          JsonConvert.SerializeObject(3.0),
                                                          Encoding.UTF8,
                                                          "application/json");

                    var stringContent2 = new StringContent(
                                          JsonConvert.SerializeObject(new { Username, Password }),
                                          Encoding.UTF8,
                                          "application/json");

                    //var response =  viewModel.ClientHTTP.PostAsync(uri, stringContent).Result;
                    var response = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        //"{\"APIHighVersion\":1,\"APILowVersion\":1,\"FocalPtVersion\":{\"_Build\":3,\"_Major\":2,\"_Minor\":55,\"_Revision\":-1},\"LocalIP\":\"172.24.64.1\"}"
                        string content = response.Content.ReadAsStringAsync().Result;
                        //var compatible = JsonConvert.DeserializeObject<Visum.Services.Mobile.Entities.CompatibleResults>(content); //JsonSerializer.Deserialize<List<CompatibleResults>>(content);
                        //if (compatible.APIHighVersion <= 3 && compatible.APILowVersion >= 1)
                        //{
                        response = ClientHTTP.PostAsync(uri2, stringContent2).Result;
                        var content2 = response.Content.ReadAsStringAsync().Result.ToString();
                        var token = JsonConvert.DeserializeObject<Guid>(content2).ToString();
                        //token = "35cb7e13-526d-401b-9295-0a5f16c0dee2";
                        if (token == "00000000-0000-0000-0000-000000000000")
                        {
                            //invalid token
                            return 3;
                        }
                        else
                        {
                            CurToken = token;
                            return 5;
                        }

                        //}
                        //else 86400000ms DNS query, check to see if on same network and use local instead of public connection. 
                        //{
                        //    //invalid version
                        //    return 2;
                        //    //  await App.Current.MainPage.DisplayAlert("Version Incompatibale with Server", "OK");
                        //}
                         ;//JsonSerializer.Deserialize<List<TodoItem>>(content, serializerOptions);
                    }
                    else
                    {
                        //invalid connection
                        return 1;
                    }
                }
                //placeholder
                return 1;
            }
            catch (Exception ex)
            {
                //await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                Console.WriteLine(ex.Message);
                return 0;
            }
            //viewModel.ValidationResultText = $"Login {Login} is correct";
        }

        internal string GetTerminalFromArray(string loginTerminal)
        {
            if (terminalDict.ContainsKey(loginTerminal))
            {
                return terminalDict[loginTerminal].TerminalNo.ToString();
            }
            return "";
        }
        internal string GetStoreFromArray(string loginStore)
        {
            if (storeDict.ContainsKey(loginStore))
            {
                return storeDict[loginStore].CmpNo.ToString();
            }
            return "";
        }

        Dictionary<string, Company> storeDict = new Dictionary<string, Company>();
        internal string[] GetStoresArray()
        {
            string baseURL = this.Model.ConnectionURL + ":" + this.Model.ConnectionPort;
            if (!baseURL.Contains("https://"))
                baseURL = "https://" + baseURL;

            List<string> CurrentStores = new List<string>();
            Uri uriStores = new Uri(string.Format(baseURL + "/Mobile/V1/LoginStores"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
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

            }
            return CurrentStores.ToArray();
        }
        Dictionary<string, Terminal> terminalDict = new Dictionary<string, Terminal>();
        internal string[] GetTerminalArray()
        {
            // if store was selected but no terminal remove the headder and readd the new one
            if (ClientHTTP.DefaultRequestHeaders.Contains("StoreNo"))
                ClientHTTP.DefaultRequestHeaders.Remove("StoreNo");
            if (!ClientHTTP.DefaultRequestHeaders.Contains("StoreNo"))
                ClientHTTP.DefaultRequestHeaders.Add("StoreNo", StoreLoginNo);

            string baseURL = this.Model.ConnectionURL + ":" + this.Model.ConnectionPort;
            if (!baseURL.Contains("https://"))
                baseURL = "https://" + baseURL;

            List<string> CurrentTerminals = new List<string>();
            Uri uriStores = new Uri(string.Format(baseURL + "/Mobile/V1//Terminals"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
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

        internal bool CheckLicenses(DataFormView dataForm)
        {
            string baseURL = this.Model.ConnectionURL + ":" + this.Model.ConnectionPort;
            if (!baseURL.Contains("https://"))
                baseURL = "https://" + baseURL;
            Uri uri3 = new Uri(string.Format(baseURL + "/Mobile/V1/ConnectionLimit"));
            string Username = this.Model.Login;
            string FingerPrint = getFingerPrint();
            short Type = GetDeviceType();
            string Phone = "";
            var stringContent3 = new StringContent(
      JsonConvert.SerializeObject(new { FingerPrint, Type, Username, Phone }),
      Encoding.UTF8,
      "application/json");

            FingerPrint = getFingerPrint();
            Type = GetDeviceType();
            Phone = getPhone();
            //add headers to client AMG popup which store and terminal to connect to
            if (ClientHTTP.DefaultRequestHeaders.Contains("Token"))
                ClientHTTP.DefaultRequestHeaders.Remove("Token");
            if (!ClientHTTP.DefaultRequestHeaders.Contains("Token"))
                ClientHTTP.DefaultRequestHeaders.Add("Token", CurToken);

            var response = ClientHTTP.PostAsync(uri3, stringContent3).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                // string content3 = response.Content.ReadAsStringAsync().Result;
                // var conLimit = JsonConvert.DeserializeObject<bool>(content3);
                //if (conLimit == false)
                // {

                return true;
            }
            else
            {
                //invalid users
                return false;
            }
            return false;

        }
        internal bool LoginSecurity()
        {
            string baseURL = this.Model.ConnectionURL + ":" + this.Model.ConnectionPort;
            if (!baseURL.Contains("https://"))
                baseURL = "https://" + baseURL;
            Uri uriSec = new Uri(string.Format(baseURL + "/Mobile/V1/LoginSecurity"));
            List<Security> secAreas = new List<Security>();

            var response = ClientHTTP.GetAsync(uriSec).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result.ToString();
                var allowedAreas = JsonConvert.DeserializeObject<List<Security>>(content);
                foreach (var area in allowedAreas)
                {
                    secAreas.Add(area);
                    // DataManager.SecurityAreas.
                }
                //DataManager.SecurityAreas = ;
                //}
                // else

                if (!ClientHTTP.DefaultRequestHeaders.Contains("StoreNo"))
                    ClientHTTP.DefaultRequestHeaders.Add("StoreNo", StoreLoginNo);
                if (!ClientHTTP.DefaultRequestHeaders.Contains("TerminalNo"))
                    ClientHTTP.DefaultRequestHeaders.Add("TerminalNo", TerminalNo);

                DataManager.Settings.UserName = this.Model.Login;
                DataManager.Settings.ApiUri = baseURL + "/Mobile/V1/";
                DataManager.Settings.HomeStore = int.Parse(StoreLoginNo);
                DataManager.Settings.Terminal = int.Parse(TerminalNo);
                DataManager.Settings.UserToken = CurToken;
                // NEEDS SQLite_Android.cs Implementation  DataManager.SaveSettings();
                DataManager.SaveSettings();

                var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
                httpClientCache.BaseUrl = (baseURL + "/Mobile/V1/");
                httpClientCache.Store = StoreLoginNo;
                httpClientCache.Terminal = TerminalNo;
                httpClientCache.Token = CurToken;
                httpClientCache.AddClient(baseURL, StoreLoginNo, TerminalNo, CurToken, ClientHTTP);
                this.clientHttp = httpClientCache.GetHttpClientAsync();
                return true;
            }
            return false;
        }

        private string getPhone()
        {
            var idiom = DeviceInfo.Idiom;
            if (idiom == DeviceIdiom.Phone)
            {

                var platform = DeviceInfo.Platform;
                if (platform == DevicePlatform.Android)
                {
                    try
                    {
                        //    TelephonyManager mgr =
                        //Android.App.Application.Context.GetSystemService(Context.TelephonyService) as TelephonyManager;
                        //    return mgr.Line1Number;
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                else if (platform == DevicePlatform.iOS)
                {
                    //Will reject if we return the phone number We will have to ask for phone number if there is one
                    return "";
                }
            }
            else
            {
                // no phone number may be unknown or tablet
                return "";
            }
            return "";
        }

        private short GetDeviceType()
        {
            var idiom = DeviceInfo.Idiom;
            if (idiom == DeviceIdiom.Phone)
            {
                return 1;
            }
            else if (idiom == DeviceIdiom.Tablet)
            {
                return 3;
            }
            else if (idiom == DeviceIdiom.Desktop)
            {
                return 5;
            }
            else if (idiom == DeviceIdiom.Unknown)
            {
                return 7;
            }
            return 7;
        }

        private string getFingerPrint()
        {
            //string id = string.Empty;
            //    if (!string.IsNullOrWhiteSpace(id))
            //        return id;

            return DependencyService.Resolve<IDeviceInfo>().DeviceId;

            //var platform = DeviceInfo.Platform;
            //if (platform == DevicePlatform.Android)
            //{

            //   // Android.Provider.Settings.Secure.GetString();
            //}
            //id = Android.OS.Build.Serial;
            //if (string.IsNullOrWhiteSpace(id) || id == Build.Unknown || id == "0")
            //{
            //    try
            //    {
            //        var context = Android.App.Application.Context;
            //        id = Secure.GetString(context.ContentResolver, Secure.AndroidId);
            //    }
            //    catch (Exception ex)
            //    {
            //        Android.Util.Log.Warn("DeviceInfo", "Unable to get id: " + ex.ToString());
            //    }
            //}
            //else if (platform == DevicePlatform.iOS)
            //{
            //    //id => UIDevice.CurrentDevice.IdentifierForVendor.AsString();
            //}

            //return id;
        }

        internal void SetSecures()
        {
            SettingsComponent.SetSecure(Ultils.HOSTKEY, Model.ConnectionURL);
            SettingsComponent.SetSecure(Ultils.PORTKEY, Model.ConnectionPort);
        }
    }
    public class ValidationCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        LoginPageViewModel viewModel;
        public ValidationCommand(LoginPageViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object Model)
        {
            if (string.IsNullOrEmpty((string)viewModel.Model.Login) || viewModel.Model.Login == null)
            {
                return true;
            }
            //viewModel.ValidationResultText = "";
            return true;
        }
        public void ShowError(object Model)
        {
        }
        public void Execute(object Login)
        {
            try
            {

                //Uri uri = new Uri(string.Format("https://10.0.2.2/RestServiceDemo/restservice/"));
                // var response = await clientHttp.GetAsync(uri);
                Uri uri = new Uri(string.Format("https://10.0.2.2:56883/Mobile/Compatible"));
                Uri uri2 = new Uri(string.Format("https://10.0.2.2:56883/Mobile/Login"));
                Uri uri3 = new Uri(string.Format("https://10.0.2.2:56883/Mobile/V1/ConnectionLimit"));
                var stringContent = new StringContent(
                                                      JsonConvert.SerializeObject(3.0),
                                                      Encoding.UTF8,
                                                      "application/json");
                string Password = "+Eg/bz+kAL5rp1moMUdS7B5o1MQZPNxbvi3bdu05huI=";
                string Username = viewModel.Model.Login;
                var stringContent2 = new StringContent(
                                      JsonConvert.SerializeObject(new { Username, Password }),
                                      Encoding.UTF8,
                                      "application/json");
                //var response =  viewModel.ClientHTTP.PostAsync(uri, stringContent).Result;
                var response = viewModel.ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    //"{\"APIHighVersion\":1,\"APILowVersion\":1,\"FocalPtVersion\":{\"_Build\":3,\"_Major\":2,\"_Minor\":55,\"_Revision\":-1},\"LocalIP\":\"172.24.64.1\"}"
                    string content = response.Content.ReadAsStringAsync().Result;
                    var compatible = JsonConvert.DeserializeObject<Visum.Services.Mobile.Entities.CompatibleResults>(content); //JsonSerializer.Deserialize<List<CompatibleResults>>(content);
                    if (compatible.APIHighVersion <= 3 && compatible.APILowVersion >= 1)
                    {
                        response = viewModel.ClientHTTP.PostAsync(uri2, stringContent2).Result;
                        var content2 = response.Content.ReadAsStringAsync().Result.ToString();
                        var token = JsonConvert.DeserializeObject<Guid>(content2).ToString();
                        if (token == "00000000-0000-0000-0000-000000000000")
                        {
                            //viewModel.ValidationResultText = "Username or password is incorrect";
                        }
                        DataManager.Settings.UserToken = token;
                    }
                    else
                    {
                        //  await App.Current.MainPage.DisplayAlert("Version Incompatibale with Server", "OK");
                    }
                     ;//JsonSerializer.Deserialize<List<TodoItem>>(content, serializerOptions);
                }
            }
            catch (Exception ex)
            {
                //await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            //viewModel.ValidationResultText = $"Login {Login} is correct";
        }
    }
}
