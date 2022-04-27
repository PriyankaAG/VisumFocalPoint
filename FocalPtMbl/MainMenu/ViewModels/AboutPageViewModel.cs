using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using FocalPoint.Utils;
using FocalPtMbl.MainMenu.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FocalPtMbl.MainMenu.ViewModels
{
    public class AboutPageViewModel : ThemeBaseViewModel
    {
        string version;
        public string ProductTitle => "FocalPoint Mobile";
        public string Version => version;
        public string CompanyUrl => "http://visum-corp.com/";
        public string ContactUrl => "http://visum-corp.com/contact/";
        public string GoogleUrl => "https://www.google.com/";

        public string CompanyEmail = "Support@Visum-Corp.com";
        public string CompanyPhone = "(763)244-8050";
        public ICommand OpenWebCommand { get; }

        public ILoginComponent LoginComponent { get; set; }

        public AboutPageViewModel(IOpenUriService openService)
        {
            InitVersion();
            OpenWebCommand = new DelegateCommand<String>((p) => openService.Open(p));
            OpenPhoneDialerCommand = new Command<string>(async phoneNo => await OpenPhoneDialerTask(phoneNo));
            OpenEmailApplicationCommand = new Command<string>(async address => await OpenEmailApplicationTask(address));
            LoginComponent = new LoginComponent();
        }
        void InitVersion()
        {
            Version assemblyVersion = Assembly.GetAssembly(this.GetType()).GetName().Version;
            version = $"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}";
        }

        #region OpenPhoneDialer

        public ICommand OpenPhoneDialerCommand { get; }

        private async Task OpenPhoneDialerTask(string phoneNumber)
        {
            try
            {
                await Utils.OpenPhoneDialer(CompanyPhone);
            }
            catch (Exception exception)
            {
                //TODO: Log Error
            }
            finally
            {
            }
        }

        #endregion OpenPhoneDialer

        #region OpenEmailApplication

        public ICommand OpenEmailApplicationCommand { get; }

        private async Task OpenEmailApplicationTask(string emailAddress)
        {
            try
            {
                await Utils.OpenEmailApplication(string.Empty, string.Empty, new List<string> { CompanyEmail });
            }
            catch (Exception exception)
            {
                //TODO: Log error
            }
            finally
            {
            }
        }

        #endregion OpenEmailApplication

        internal async void Logoff()
        {
            try
            {
                bool isLoggedOut = await LoginComponent.Logoff();
            }
            catch 
            { 
            }
        }
    }
}
