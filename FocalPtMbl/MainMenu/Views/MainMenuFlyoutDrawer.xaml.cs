using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.MainMenu.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.MainMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuFlyoutDrawer : ContentPage
    {
        public ListView ListView;
        public MainMenuFlyoutDrawer()
        {
            InitializeComponent();

            ListView = MenuItemsListView;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            (BindingContext as MainMenuFlyoutDrawerViewModel).NavigationObject = Navigation;
        }
    }
}