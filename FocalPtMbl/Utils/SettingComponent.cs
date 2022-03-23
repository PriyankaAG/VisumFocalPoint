using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FocalPoint.Utils
{
    public static class SettingComponent
    {
        public static void AddSettings(string key, string value)
        {
            SecureStorage.SetAsync(key, value);
        }
        public static async Task<string> GetSettings(string key)
        {
            var result = await SecureStorage.GetAsync(key);
            return result;
        }
    }
}
