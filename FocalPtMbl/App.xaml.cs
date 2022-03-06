using DevExpress.XamarinForms.Core.Themes;
using FocalPoint.Data;
using FocalPoint.Data.DataModel;
using FocalPoint.MainMenu.Services;
using FocalPoint.MainMenu.Views;
using FocalPoint.Utils;
using FocalPtMbl.MainMenu.Services;
using FocalPtMbl.MainMenu.ViewModels;
using FocalPtMbl.MainMenu.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;


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


            this.navigationService = new NavigationService();
            this.navigationService.PageBinders.Add(typeof(ControlPageViewModel), () => new ControlPage());

            MainPageViewModel mainPageViewModel = new MainPageViewModel(this.navigationService);
            AboutPageViewModel aboutPageViewModel = new AboutPageViewModel(new XFUriOpener());
            BasePage basePage = new BasePage();
            basePage.MainContent.BindingContext = mainPageViewModel;
            basePage.DrawerContent.BindingContext = aboutPageViewModel;
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
                if (!string.IsNullOrWhiteSpace(DataManager.Settings?.UserToken))
                {
                    LoadMainPage();
                }
                else
                {
                    
                    basePage.Navigation.PushModalAsync(new LoginPageView());
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void LoadMainPage()
        {
            DataManager.LoadHttpClientCache();
            MainPageViewModel mainPageViewModel = new MainPageViewModel(navigationService, true);
            AboutPageViewModel aboutPageViewModel = new AboutPageViewModel(new XFUriOpener());
            BasePage basePage = new BasePage();
            basePage.MainContent.BindingContext = mainPageViewModel;
            basePage.DrawerContent.BindingContext = aboutPageViewModel;
            Xamarin.Forms.Application.Current.MainPage = basePage;
            this.navigationService.SetNavigator(basePage.NavPage);
            ThemeLoader.Instance.LoadTheme();
        }

        //public async void ProcessNotificationIfNeed(Guid reminderId, int recurrenceIndex)
        //{
        //    if (reminderId == Guid.Empty)
        //        return;
        //    IEnumerable<Page> openedPages = this.navigationService.GetOpenedPages<RemindersDemo>();
        //    RemindersDemo remindersDemo = (openedPages.Any() ? openedPages.Last() : await this.navigationService.PushPage(SchedulerData.GetItem(typeof(RemindersDemo)))) as RemindersDemo;
        //    remindersDemo?.OpenAppointmentEditForm(reminderId, recurrenceIndex);
        //}
        protected override async void OnStart()
        {
            base.OnStart();
            bool lightTheme = true;//await DependencyService.Get<IEnvironment>().IsLightOperatingSystemTheme();
            ApplyTheme(lightTheme);
        }

        protected override void OnSleep()
        {
        }

        protected override async void OnResume()
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
                AboutPageViewModel aboutPageViewModel = new AboutPageViewModel(new XFUriOpener());
                BasePage basePage = new BasePage();
                basePage.MainContent.BindingContext = mainPageViewModel;
                basePage.DrawerContent.BindingContext = aboutPageViewModel;
                MainPage = basePage;
            }
        }
    }
}

