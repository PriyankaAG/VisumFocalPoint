using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FocalPoint
{
    public class SettingsComponent: ISettingsComponent
    {
        public async Task<string> GetSecure(string key)
        {
            string value = null;
            try
            {
                value = await SecureStorage.GetAsync(key);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                TrackError(exception);
            }
            return value;
        }

        public async Task SetSecure(string key, string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    SecureStorage.Remove(key);
                }
                else
                {
                    await SecureStorage.SetAsync(key, value);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                TrackError(exception);
            }
        }

        public void RemoveSecure(string key)
        {
            try
            {
                SecureStorage.Remove(key);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                TrackError(exception);
            }
        }

        public void ClearSecure()
        {
            try
            {
                SecureStorage.RemoveAll();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                TrackError(exception);
            }
        }

        public void TrackError(Exception exception, IDictionary<string, string> properties = null, [CallerMemberName] string caller = null)
        {
            if (caller != null)
            {
                if (properties == null)
                {
                    properties = new Dictionary<string, string>();
                }
                properties.Add("Caller", caller);
            }
            //TODO: Log Error on App Center
        }
    }
}
