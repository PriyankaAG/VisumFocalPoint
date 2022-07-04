using DevExpress.XamarinForms.Core.Themes;
using FocalPoint;
using FocalPoint.Data;
using FocalPoint.Data.DataModel;
using FocalPoint.MainMenu.Services;
using FocalPoint.MainMenu.ViewModels;
using FocalPoint.MainMenu.Views;
using FocalPoint.Modules.FrontCounter.ViewModels;
using FocalPoint.Modules.FrontCounter.Views;
using FocalPoint.Modules.Payments.Views;
using FocalPoint.Utils;
using FocalPtMbl.MainMenu.Services;
using FocalPtMbl.MainMenu.ViewModels;
using FocalPtMbl.MainMenu.ViewModels.Services;
using FocalPtMbl.MainMenu.Views;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

[assembly: ExportFont("Roboto-Bold.ttf", Alias = "Roboto-Bold")]
[assembly: ExportFont("Roboto-Medium.ttf", Alias = "Roboto-Medium")]
[assembly: ExportFont("Roboto-Regular.ttf", Alias = "Roboto-Regular")]
[assembly: ExportFont("Roboto-Light.ttf", Alias = "Roboto-Light")]
[assembly: ExportFont("Roboto-Italic.ttf", Alias = "Roboto-Italic")]
namespace FocalPtMbl
{
    public partial class App : Xamarin.Forms.Application
    {
        readonly NavigationService navigationService;
        bool themeIsSetting;
        internal event EventHandler ThemeChagedEvent;

        public DataManager Database;
        public App()
        {

            //ioc
            //Ioc.RegisterServices();
            DependencyService.RegisterSingleton<ICrypt>(new Crypt());
            DependencyService.RegisterSingleton<IHttpClientCacheService>(new HttpClientCacheService());
            //DependencyService.RegisterSingleton<ISQLite>(new );


            //var httpClientCache = DependencyService.Resolve<IHttpClientCacheService>();
            //httpClientCache.BaseUrl = "https://visumaaron-local.fpsdns.com:56883/Mobile/V1/";
            //httpClientCache.Store = "1";
            //httpClientCache.Terminal = "3";
            //httpClientCache.Token = "0277b6a8-d525-421a-aab0-63d74a56b76b";
            //Creating database
            DataManager dataManager = new DataManager();
            DataManager.LoadSettings(Convert.ToString(Xamarin.Essentials.AppInfo.Version));
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
            if (DataManager.Settings == null)
                DataManager.Settings = new Settings();

            if (DataManager.Settings.ApiVersion == 0)
                DataManager.Settings.ApiVersion = 1;

            if (DataManager.Settings.DbVersion == 0)
                DataManager.Settings.DbVersion = 1;

            if (DataManager.Settings.ApiSvcVersion != 3)
            {
                //Try 4 first, until the API returns back a svc api version.
                DataManager.Settings.ApiSvcVersion = 4;
            }
            //set URI strings here if dev https://stackoverflow.com/questions/8732307/does-xaml-have-a-conditional-compiler-directive-for-debug-mode
            //DataManager.Settings.ApiUri = "https://visumaaron-local.fpsdns.com:56883/Mobile/V1/";
            if (!DataManager.Settings.IsSignedIn)
            {
                DataManager.Settings = new Settings();
            }
            this.navigationService = new NavigationService();
            this.navigationService.PageBinders.Add(typeof(ControlPageViewModel), () => new ControlPage());

            DataManager.LoadHttpClientCache();
            MainPageViewModel mainPageViewModel = new MainPageViewModel(this.navigationService);
            MainMenuFlyoutDrawerViewModel drawerPageViewModel = new MainMenuFlyoutDrawerViewModel(new XFUriOpener());
            MainMenuFlyout basePage = new MainMenuFlyout();
            basePage.MainPageObject.BindingContext = mainPageViewModel;
            basePage.FlyoutPageDrawerObject.BindingContext = drawerPageViewModel;

            try
            {
                MainPage = basePage;
                this.navigationService.SetNavigator(basePage.NavPage);
                DevExpress.XamarinForms.CollectionView.Initializer.Init();
                DevExpress.XamarinForms.DataForm.Initializer.Init();
                DevExpress.XamarinForms.DataGrid.Initializer.Init(); //
                DevExpress.XamarinForms.Editors.Initializer.Init();
                DevExpress.XamarinForms.Navigation.Initializer.Init();
                InitializeComponent();
                ThemeLoader.Instance.LoadTheme();
                if (!(DataManager.Settings?.IsSignedIn ?? false && IsLicensesValid()))
                {
                    MainPage.Navigation.PushModalAsync(new LoginPageNew());
                }
                DependencyService.RegisterSingleton<INavigationService>(this.navigationService);
            }
            catch (Exception ex)
            {

            }
        }

        internal bool IsLicensesValid()
        {
            string FingerPrint = DependencyService.Resolve<IDeviceInfo>().DeviceId;
            short Type = Utils.GetDeviceType();
            string Phone = "";

            var httpClientCache = DependencyService.Resolve<IHttpClientCacheService>();
            var clientHttp = httpClientCache.GetHttpClientAsync();
            var uri = DataManager.Settings.ApiUri + "ConnectionLimit";

            if (clientHttp.DefaultRequestHeaders.Contains("User"))
                clientHttp.DefaultRequestHeaders.Remove("User");
            clientHttp.DefaultRequestHeaders.Add("User", DataManager.Settings.User);

            if (clientHttp.DefaultRequestHeaders.Contains("Token"))
                clientHttp.DefaultRequestHeaders.Remove("Token");

            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(new { FingerPrint, Type, Phone }), Encoding.UTF8, "application/json");
                var response = clientHttp.PostAsync(uri, stringContent).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result.ToString();
                    var token = JsonConvert.DeserializeObject<Guid>(content);
                    if (token == Guid.Empty)
                    {
                        return false;
                    }
                    else
                    {
                        var Token = token.ToString();
                        clientHttp.DefaultRequestHeaders.Add("Token", Token);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        protected override void OnStart()
        {
            base.OnStart();
            bool lightTheme = true;//await DependencyService.Get<IEnvironment>().IsLightOperatingSystemTheme();
            ApplyTheme(lightTheme);
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            base.OnResume();
            //if (!this.themeIsSetting)
            //{
            //    bool lightTheme = await DependencyService.Get<IEnvironment>().IsLightOperatingSystemTheme();
            //    ApplyTheme(lightTheme);
            //}
        }
        void ApplyTheme(bool isLightTheme)
        {
            ThemeManager.ThemeName = isLightTheme ? Theme.Light : Theme.Dark;
            ThemeChagedEvent?.Invoke(this, new EventArgs());
        }
        internal void ApplyTheme(bool isLightTheme, bool force)
        {
            if (force)
            {
                ApplyTheme(isLightTheme);
                this.themeIsSetting = true;
            }
        }
        public void applyPermisionSettings(bool changePermissions)
        {
            if (changePermissions)
            {
                MainPageViewModel mainPageViewModel = new MainPageViewModel(this.navigationService);
                MainMenuFlyoutDrawerViewModel drawerPageViewModel = new MainMenuFlyoutDrawerViewModel(new XFUriOpener());
                MainMenuFlyout basePage = new MainMenuFlyout();
                basePage.MainPageObject.BindingContext = mainPageViewModel;
                basePage.FlyoutPageDrawerObject.BindingContext = drawerPageViewModel;

                MainPage = basePage;
            }
        }
    }
}

