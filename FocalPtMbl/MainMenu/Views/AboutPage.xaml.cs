using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DevExpress.XamarinForms.Editors;
using FocalPoint.MainMenu.Views;
using Xamarin.Essentials;

namespace FocalPtMbl.MainMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ScrollView
    {
        public static readonly BindableProperty OpenedByParentProperty = BindableProperty.Create(nameof(OpenedByParent), typeof(bool), typeof(AboutPage), defaultValue: false);
        public AboutPage()
        {
            InitializeComponent();
        }
        public bool OpenedByParent
        {
            get => (bool)GetValue(OpenedByParentProperty);
            set => SetValue(OpenedByParentProperty, value);
        }

        private async void ChangeStore_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ChangeStoreView());
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            bool loggedOut = await App.Current.MainPage.DisplayAlert("Logout", "Are you sure you want to logout", "Ok", "Cancel");
            if (loggedOut)
            {

                await Navigation.PushModalAsync(new LoginPageView());

                // await Navigation.PushModalAsync(new SplashPage());
            }
        }
        private async void Settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SettingsPageView());
        }

        private async void TimeClock_Clicked(object sender, EventArgs e)
        {
            //await OpenDetailPage();
            await Navigation.PushModalAsync(new TimeClockView());
        }
        Task OpenDetailPage()
        {

            return Navigation.PushAsync(new TimeClockView());
        }
    }
}