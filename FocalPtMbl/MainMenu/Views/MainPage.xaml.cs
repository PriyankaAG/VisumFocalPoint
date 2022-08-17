
using DevExpress.XamarinForms.Editors;
using DevExpress.XamarinForms.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FocalPtMbl.MainMenu.ViewModels;
using FocalPoint.Modules.FrontCounter.Views;
using FocalPoint.MainMenu.Views;

namespace FocalPtMbl.MainMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            TitleViewExtensions.SetIsShadowVisible(this, false);
            LoadHttpClient();
        }

        private void LoadHttpClient()
        {
            //((MainPageViewModel)this.BindingContext).LoadHttpClient();

            //throw new NotImplementedException();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if ((Application.Current.MainPage as MainMenuFlyout).IsDashboardAboutToLoad == true) 
            {
                try
                {
                    var a = ((Content as Grid).Children[0] as FrontCounter);
                    a?.ReloadData();

                    (Application.Current.MainPage as MainMenuFlyout).IsDashboardAboutToLoad = false;
                }
                catch (Exception ex)
                {
                }
            }

        }

        private void OnInfoClicked(object sender, EventArgs e)
        {
            (Application.Current.MainPage as DrawerPage).IsDrawerOpened = true;
        }
        public void Pagetem_TappedControlShortcut(object sender, System.EventArgs e)
        {
            var groupItemView = (GroupItemView)sender;
            if (BindingContext is MainPageViewModel viewModel && groupItemView != null)
            {
                if (viewModel.NavigationPageCommand != null)
                {
                    viewModel.NavigationPageCommand.Execute(groupItemView.BindingContext);
                }
            }
        }
    }
}