using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FocalPoint.MainMenu.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Android.Provider.Settings;

namespace FocalPtMbl.Droid
{
    public class DeviceInfo : IDeviceInfo
    {
        private string deviceId;
        private string modelNumber;
        private string manufacturer;

        public string DeviceId => this.deviceId;

        public string ModelNumber => this.modelNumber;

        public string Manufacturer => this.manufacturer;
        public DeviceInfo()
        {
            this.deviceId = GetPhoneUId();
            this.modelNumber = string.Empty;
            this.manufacturer = string.Empty;
        }
        private string GetPhoneUId()
        {
            var context = Android.App.Application.Context;
            return Secure.GetString(context.ContentResolver, Secure.AndroidId);
        }
    }
}