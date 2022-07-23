using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FocalPtMbl.MainMenu.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.MainMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuFlyout : FlyoutPage
    {
        public NavigationPage NavPage => navPage;
        public MainPage MainPageObject => mainPage;
        public MainMenuFlyoutDrawer FlyoutPageDrawerObject => FlyoutPageDrawer;
        public bool IsQuickRentalScreenDisplaying { get; set; } = false;
        public MainMenuFlyout()
        {
            InitializeComponent();
            FlyoutPageDrawer.ListView.ItemSelected += ListView_ItemSelected;
            IsGestureEnabled = true;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        protected override bool OnBackButtonPressed()
        {
            bool result = true;
            if (IsQuickRentalScreenDisplaying)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await this.DisplayAlert("Alert!", "Please Save or Void the order to exit.", "Ok");
                });
                result = true;
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    result = await this.DisplayAlert("Alert!", "Are you sure you want to Exit?", "Yes", "No");
                });
            }
            return result;

        }
        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainMenuFlyoutFlyoutMenuItem;
            if (item == null)
                return;

            Page page;

            if (item.TargetType == typeof(MainPage))
            {
                page = Application.Current.MainPage;
                page.Title = item.Title;
                Detail = ((MainMenuFlyout)Application.Current.MainPage).navPage;
                IsPresented = false;

                FlyoutPageDrawer.ListView.SelectedItem = null;
            }
            else
            {
                page = Activator.CreateInstance(item.TargetType) as Page;
                if (page != null)
                {
                    page.Title = item.Title;
                    Detail = new NavigationPage(page);
                }
                IsPresented = false;
            }
        }
    }
}