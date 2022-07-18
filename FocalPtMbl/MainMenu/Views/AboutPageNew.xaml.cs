using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.Data;
using FocalPtMbl.MainMenu.Services;
using FocalPtMbl.MainMenu.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.MainMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPageNew : ContentPage
    {
        public AboutPageNew()
        {
            InitializeComponent();
            version.Text = "Version: " + DataManager.Settings.AppVersion;
            apiSvcVersion.Text = string.Format("Server Version: {0}", DataManager.Settings.ApiSvcVersion);
            apiVersion.Text = string.Format("Server API Version: {0}", DataManager.Settings.ApiVersion);
            dbVersion.Text = string.Format("Database Version: {0}", DataManager.Settings.DbVersion);
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Utils.Utils.OpenPhoneDialer("(763)244-8050");
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            var service = new XFUriOpener();
            service.Open("http://visum-corp.com/");
        }

        private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            await Utils.Utils.OpenEmailApplication(string.Empty, string.Empty, new List<string> { "Support@Visum-Corp.com" });

        }
    }
}