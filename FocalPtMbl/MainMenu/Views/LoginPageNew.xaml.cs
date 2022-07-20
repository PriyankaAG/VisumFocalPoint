using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.MainMenu.ViewModels;
using FocalPtMbl;
using FocalPtMbl.MainMenu.Services;
using FocalPtMbl.MainMenu.ViewModels;
using FocalPtMbl.MainMenu.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.MainMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPageNew : ContentPage
    {
        readonly NavigationService navigationService;
        //ActivityIndicator activityIndicator;
        LoginPageViewModelNew viewModel;

        public LoginPageNew()
        {
            InitializeComponent();
            BindingContext = new LoginPageViewModelNew();
            navigationService = new NavigationService();
            navigationService.PageBinders.Add(typeof(ControlPageViewModel), () => new ControlPage());
            //activityIndicator = new ActivityIndicator { IsRunning = false };
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel = (LoginPageViewModelNew)this.BindingContext;

        }
        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (!viewModel.ValidateLogin()) return;
            try
            {
                var UpdateValid = viewModel.AttemptLogin();
                if (UpdateValid == 5)
                {
                    int checkLicence = viewModel.CheckLicenses();
                    switch (checkLicence)
                    {
                        case 1:
                            await ProcessLogin();
                            break;
                        case 0:
                            await Application.Current.MainPage.DisplayAlert("Number of users", "There are not enough licenses for the amount of mobile licenses active", "OK");
                            break;
                        default:
                            await Application.Current.MainPage.DisplayAlert("Focal Point", "Something went wrong. Please try again.", "OK");
                            break;
                    }
                    //await Navigation.PopModalAsync();
                }
                else if (UpdateValid == 3)
                {
                    //invalid token username and password
                    await Application.Current.MainPage.DisplayAlert("Focal Point", "Either Username and Password is incorrect or the user does not have access to the App.", "OK");
                }
                else if (UpdateValid == -1)
                {
                    await Application.Current.MainPage.DisplayAlert("Focal Point", "The URL or Port is incorrect or the server service is faulted", "OK");
                    //invalid connection
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Focal Point", "Something went wrong. Please try again.", "OK");
            }
        }

        private async Task ProcessLogin()
        {
            viewModel.Indicator = true;
            string[] stores = viewModel.GetStoresArray();
            string currentSelectedLoginStore;
            viewModel.Indicator = false;
            if (stores?.Count() == 1)
            {
                currentSelectedLoginStore = stores[0];
            }
            else
            {
                currentSelectedLoginStore = await DisplayActionSheet("Select Store:", "Cancel", null, stores);
            }
            if (currentSelectedLoginStore == "Cancel")
            {
                await Application.Current.MainPage.DisplayAlert("Must Select Store", "The store must be selected to continue", "OK");
                return;
            }
            viewModel.Indicator = true;
            viewModel.StoreLoginNo = viewModel.GetStoreFromArray(currentSelectedLoginStore);
            string[] terminals = viewModel.GetTerminalArray();
            string currentSelectedLoginTerminal;
            viewModel.Indicator = false;
            if (terminals?.Count() == 1)
            {
                currentSelectedLoginTerminal = terminals[0];
            }
            else
            {
                currentSelectedLoginTerminal = await DisplayActionSheet("Select Terminal:", "Cancel", null, terminals);
            }
            viewModel.TerminalNo = viewModel.GetTerminalFromArray(currentSelectedLoginTerminal);
            
            if (currentSelectedLoginTerminal == "Cancel")
            {
                await Application.Current.MainPage.DisplayAlert("Must Select Terminal", "The terminal must be selected to continue", "OK");
                return;
            }


            // start loading
            if (!(currentSelectedLoginTerminal == "Cancel" || currentSelectedLoginStore == "Cancel"))
            {
                activityIndicator.IsRunning = true;
                if (viewModel.LoginSecurity())
                {
                    ShowMainPage();
                }
            }
        }

        private void ShowMainPage()
        {
            MainPageViewModel mainPageViewModel = new MainPageViewModel(this.navigationService);
            MainMenuFlyoutDrawerViewModel drawerPageViewModel = new MainMenuFlyoutDrawerViewModel(new XFUriOpener());
            MainMenuFlyout basePage = new MainMenuFlyout();
            basePage.MainPageObject.BindingContext = mainPageViewModel;
            basePage.FlyoutPageDrawerObject.BindingContext = drawerPageViewModel;

            Application.Current.MainPage = basePage;
            this.navigationService.SetNavigator(basePage.NavPage);
            ThemeLoader.Instance.LoadTheme();
            //end loading
            activityIndicator.IsRunning = false;

            //Login is successful and terminal/stops have been selected.
            //Save the host and port on the device to load later
            viewModel.SetSecures();
        }

        private void IsSignedInCheckbox_IsCheckedChanged(object sender, TappedEventArgs e)
        {
            viewModel.IsSignedIn = !viewModel.IsSignedIn;
        }
    }
}