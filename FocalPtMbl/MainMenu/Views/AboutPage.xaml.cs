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
            if(loggedOut)
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
        public async Task SendEmail(string subject, string body, List<string> recipients)
        {
            try
            {
                recipients.Add("Support@Visum-Corp.com");
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }
        public async Task CallNumber()
        {
            try
            {
                PhoneDialer.Open("7632448050");
                //return CompletedTask;
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await CallNumber();
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            await SendEmail("", "", new List<string>());
        }
    }
}