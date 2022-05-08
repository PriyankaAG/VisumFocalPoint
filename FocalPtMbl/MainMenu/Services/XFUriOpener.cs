using FocalPtMbl.MainMenu.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace FocalPtMbl.MainMenu.Services
{
    public class XFUriOpener : IOpenUriService 
    {
        public void Open(string uri)
        {
            Launcher.OpenAsync(new Uri(uri));
        }
    }
}
