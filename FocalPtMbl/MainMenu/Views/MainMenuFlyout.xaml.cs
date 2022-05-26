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
        public MainMenuFlyout()
        {
            InitializeComponent();
            FlyoutPageDrawer.ListView.ItemSelected += ListView_ItemSelected;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
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
                page = (Page)Activator.CreateInstance(item.TargetType);
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