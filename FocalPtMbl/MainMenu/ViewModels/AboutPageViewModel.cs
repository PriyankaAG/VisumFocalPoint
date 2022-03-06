using FocalPtMbl.MainMenu.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Input;

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
        public ICommand OpenWebCommand { get; }

        public AboutPageViewModel(IOpenUriService openService)
        {
            InitVersion();
            OpenWebCommand = new DelegateCommand<String>((p) => openService.Open(p));
        }
        void InitVersion()
        {
            Version assemblyVersion = Assembly.GetAssembly(this.GetType()).GetName().Version;
            version = $"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}";
        }
    }
}
