using DevExpress.XamarinForms.DataForm;
using FocalPoint.MainMenu.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using FocalPtMbl;
using Xamarin.Essentials;
using FocalPtMbl.MainMenu.ViewModels;
using FocalPtMbl.MainMenu.Views;
using FocalPtMbl.MainMenu.Services;

namespace FocalPoint.MainMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPageView : ContentPage
    {
        static readonly string BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ?
                            "https://10.0.2.2:44344" :
                            "https://localhost:44344";
        System.Net.Http.HttpClient clientHttp = new System.Net.Http.HttpClient();
        private string user = "";
        private string password = "";
        private string url = "";
        private string port = "";
        private bool URLFailed = false;
        private bool LoginFailed = false;
        public LoginPageView()
        {
            //make a client using a factory pass the proto
            DevExpress.XamarinForms.DataForm.Initializer.Init();
            InitializeComponent();
            BindingContext = new LoginPageViewModel();
            dataForm.ValidateProperty += DataFormOnValidateProperty;
            clientHttp = PreparedClient();
            navigationService = new NavigationService();
            navigationService.PageBinders.Add(typeof(ControlPageViewModel), () => new ControlPage());
            activityIndicator = new ActivityIndicator { IsRunning = false };
        }
        ActivityIndicator activityIndicator;

        void DataFormOnValidateProperty(object sender, DataFormPropertyValidationEventArgs e)
        {
            if (e.PropertyName == "Login")
            {
                if (e.NewValue.ToString() != "")
                {
                    user = e.NewValue.ToString();
                    ((LoginPageViewModel)this.BindingContext).Model.Login = user;
                    if (LoginFailed)
                    {
                        e.ErrorText = "The Username or Password is incorrect";
                        e.HasError = true;
                    }

                    /// RCG:  Look into this ??????????
                    else if (e.HasError)
                    {
                        dataForm.Commit();
                    }
                }
            }
            if (e.PropertyName == nameof(LoginPageViewModel.Model.Password))// "Password")
            {
                if (e.NewValue.ToString() != "")
                {
                    password = e.NewValue.ToString();
                    ((LoginPageViewModel)this.BindingContext).Model.Password = password;
                    if (LoginFailed)
                    {
                        e.ErrorText = "The Username or Password is incorrect";
                        e.HasError = true;
                    }
                    else if (e.HasError)
                    {
                        dataForm.Commit();
                    }
                }
            }
            if (e.PropertyName == nameof(LoginPageViewModel.Model.ConnectionURL))
            {
                if (e.NewValue.ToString() != "")
                {
                    url = e.NewValue.ToString();
                    ((LoginPageViewModel)this.BindingContext).Model.ConnectionURL = url;
                    if (URLFailed)
                    {
                        e.ErrorText = "The URL or Port is incorrect or the server service is faulted";
                        e.HasError = true;
                    }
                    else if (e.HasError && !URLFailed)
                    {
                        dataForm.Commit();
                    }
                }
            }
            if (e.PropertyName == nameof(LoginPageViewModel.Model.ConnectionPort))
            {
                if (e.NewValue.ToString() != "")
                {
                    port = e.NewValue.ToString();
                    ((LoginPageViewModel)this.BindingContext).Model.ConnectionPort = port;
                    if (URLFailed)
                    {
                        e.ErrorText = "The URL or Port is incorrect or the server service is faulted";
                        e.HasError = true;
                    }
                    else if (e.HasError)
                    {
                        dataForm.Validate();
                    }
                }
            }
            //if (e.PropertyName == nameof(CustomerAddInfo.DeliveryTimeFrom))
            //{
            //    ((CustomerAddInfo)dataForm.DataObject).DeliveryTimeFrom = (DateTime)e.NewValue;
            //    Device.BeginInvokeOnMainThread(() => {
            //        dataForm.Validate(nameof(CustomerAddInfo.DeliveryTimeTo));
            //    });
            //}
            //if (e.PropertyName == nameof(CustomerAddInfo.DeliveryTimeTo))
            //{
            //    DateTime timeFrom = ((CustomerAddInfo)dataForm.DataObject).DeliveryTimeFrom;
            //    if (timeFrom > (DateTime)e.NewValue)
            //    {
            //        e.HasError = true;
            //        e.ErrorText = "The end time cannot be less than the start time";
            //        return;
            //    }
            //}
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

        protected override void OnSizeAllocated(double width, double height)
        {
            //((LoginPageViewModel)this.BindingContext).Rotate(dataForm, height > width);
            base.OnSizeAllocated(width, height);
        }
        protected override bool OnBackButtonPressed()
        {
            return false;
        }
        readonly NavigationService navigationService;
        Task<Location> GetLocation()
        {
            try
            {
                //Xamarin.Essentials.
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                // whats me current permissions
                var location = Geolocation.GetLocationAsync(request);
                return location;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception               
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception                
            }
            catch (Exception ex)
            {
                // Unable to get location              
            }
            //return location;
            return null;
        }
        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            try
            {

                var UpdateValid = ((LoginPageViewModel)this.BindingContext).AttemptLogin(dataForm);
                if (UpdateValid == 5)
                {
                    int checkLicence = ((LoginPageViewModel)this.BindingContext).CheckLicenses();
                    if (checkLicence == 1)
                        await ProcessLogin();
                    else if (checkLicence == 0)
                        await Application.Current.MainPage.DisplayAlert("Number of users", "There are not enough licenses for the amount of mobile licenses active", "OK");
                    else
                        await Application.Current.MainPage.DisplayAlert("Focal Point", "Something went wrong. Please try again.", "OK");
                    //await Navigation.PopModalAsync();
                }
                else if (UpdateValid == 3)
                {
                    //invalid token username and password
                    LoginFailed = true;
                    dataForm.Commit();
                    LoginFailed = false;
                    await Application.Current.MainPage.DisplayAlert("Focal Point", "Either Username and Password is incorrect or the user does not access to the App.", "OK");
                }
                else if (UpdateValid == -1)
                {
                    //invalid connection
                    URLFailed = true;
                    dataForm.Commit();
                    URLFailed = false;
                }
                /*else if (UpdateValid == 4)
                {
                    //invalid # of users
                    //await App.Current.MainPage.DisplayAlert("Number of users", "There are not enough licenses for the amount of mobile licenses active", "OK");
                }
                else if (UpdateValid == 2)
                {
                    //invalid version
                    await DisplayAlert("Invalid Version", "The version between the server and phone are incompatible, please update ", "OK");
                }*/

            }
            catch (Exception ex)
            {
                await DisplayAlert("Focal Point", "Something went wrong. Please try again.", "OK");
            }
        }

        private async Task ProcessLogin()
        {
            string[] stores = ((LoginPageViewModel)this.BindingContext).GetStoresArray();
            string currentSelectedLoginStore;
            if (stores?.Count() == 1)
            {
                currentSelectedLoginStore = stores[0];
            }
            else
            {
                currentSelectedLoginStore = await DisplayActionSheet("Select Store:", "Cancel", null, stores);
            }

                                                ((LoginPageViewModel)this.BindingContext).StoreLoginNo = ((LoginPageViewModel)this.BindingContext).GetStoreFromArray(currentSelectedLoginStore);
            string[] terminals = ((LoginPageViewModel)this.BindingContext).GetTerminalArray();
            string currentSelectedLoginTerminal;
            if (terminals?.Count() == 1)
            {
                currentSelectedLoginTerminal = terminals[0];
            }
            else
            {
                currentSelectedLoginTerminal = await DisplayActionSheet("Select Terminal:", "Cancel", null, terminals);
            }
                                                ((LoginPageViewModel)this.BindingContext).TerminalNo = ((LoginPageViewModel)this.BindingContext).GetTerminalFromArray(currentSelectedLoginTerminal);

            if (currentSelectedLoginStore == "Cancel")
            {
                await App.Current.MainPage.DisplayAlert("Must Select Store", "The store must be selected to continue", "OK");

            }
            if (currentSelectedLoginTerminal == "Cancel")
            {
                await App.Current.MainPage.DisplayAlert("Must Select Terminal", "The terminal must be selected to continue", "OK");
            }


            // start loading
            if (!(currentSelectedLoginTerminal == "Cancel" || currentSelectedLoginStore == "Cancel"))
            {
                activityIndicator.IsRunning = true;
                LoginPageViewModel LoginPageViewModel = (LoginPageViewModel)this.BindingContext;
                if (LoginPageViewModel.LoginSecurity())
                {
                    //goto main page
                    MainPageViewModel mainPageViewModel = new MainPageViewModel(navigationService, true);
                    AboutPageViewModel aboutPageViewModel = new AboutPageViewModel(new XFUriOpener());
                    BasePage basePage = new BasePage();
                    basePage.MainContent.BindingContext = mainPageViewModel;
                    basePage.DrawerContent.BindingContext = aboutPageViewModel;
                    Application.Current.MainPage = basePage;
                    this.navigationService.SetNavigator(basePage.NavPage);
                    ThemeLoader.Instance.LoadTheme();
                    //end loading
                    activityIndicator.IsRunning = false;

                    //Login is successful and terminal/stops have been selected.
                    //Save the host and port on the device to load later
                    LoginPageViewModel.SetSecures();
                }
            }
        }
    }
}