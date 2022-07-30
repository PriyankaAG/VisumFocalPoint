using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FocalPoint.Utils
{
    public class Utils
    {
        //Used to save the Host and Port after successful login
        public const string HOSTKEY = "host";
        public const string PORTKEY = "port";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ConvertPhone(long? num)
        {
            if (!num.HasValue)
                return null;

            return ConvertPhone(num.Value.ToString());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ConvertPhone(string num)
        {
            if (string.IsNullOrEmpty(num))
                return null;

            var phone = string.Format("{0:(###) ###-####}", num);

            //Appears to be a bug, that the "Format" doesn't convert it correctly, and leaves it as is
            if (phone.Length == phone.Length && phone.Length != 14 && phone.Length == 10)
                phone = string.Format("({0}) {1}-{2}", phone.Substring(0, 3), phone.Substring(3, 3), phone.Substring(6, 4));

            return phone;
        }

        public static string ConvertToBase64(Stream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            return Convert.ToBase64String(bytes);
        }

        public static string CompressImage(string base64String)
        {
            using (var outStream = new MemoryStream())
            {
                using (var tinyStream = new GZipStream(outStream, CompressionMode.Compress))
                using (var mStream = new MemoryStream(Encoding.UTF8.GetBytes(base64String)))
                    mStream.CopyTo(tinyStream);

                var compressed = outStream.ToArray();
                return Convert.ToBase64String(compressed);
            }
        }

        public static string UnCompressImage(string base64String)
        {
            try
            {
                using (var mStream = new MemoryStream())
                {
                    using (var inStream = new MemoryStream(Convert.FromBase64String(base64String)))
                    using (var bigStream = new GZipStream(inStream, CompressionMode.Decompress))
                        bigStream.CopyTo(mStream);

                    return Encoding.UTF8.GetString(mStream.ToArray());
                }
            }
            catch(Exception ex)
            {

            }
            return "";
        }

        public static async Task OpenMapApplication(string address)
        {
            try
            {
                // Do Geo-coding and get the lat/long for the address.
                IEnumerable<Xamarin.Essentials.Location> locations = await Geocoding.GetLocationsAsync(address);
                Xamarin.Essentials.Location location = locations?.FirstOrDefault();
                if (location != null)
                {
                    MapLaunchOptions options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
                    await Map.OpenAsync(location, options);
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                //TODO: Log Error
                throw;
            }
            catch (Exception ex)
            {
                //TODO: Log Error
                throw;
            }
        }

        public static async Task OpenPhoneDialer(string phoneNumber)
        {
            try
            {
                PhoneDialer.Open(phoneNumber);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                //TODO: Log Error
                throw;
            }
            catch (Exception ex)
            {
                //TODO: Log Error
                throw;
            }
        }

        public static async Task OpenEmailApplication(
            string subject,
            string body,
            List<string> recipients)
        {
            try
            {
                EmailMessage message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                //TODO: Log Error
                throw;
            }
            catch (Exception ex)
            {
                //TODO: Log Error
                throw;
            }
        }

        public static short GetDeviceType()
        {
            var idiom = DeviceInfo.Idiom;
            if (idiom == DeviceIdiom.Phone)
            {
                return 1;
            }
            else if (idiom == DeviceIdiom.Tablet)
            {
                return 3;
            }
            else if (idiom == DeviceIdiom.Desktop)
            {
                return 5;
            }
            else if (idiom == DeviceIdiom.Unknown)
            {
                return 7;
            }
            return 7;
        }

        public static string GetDescriptionFromEnumValue(Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            FieldInfo[] fields = type.GetFields();
            var field = fields
                            .SelectMany(f => f.GetCustomAttributes(
                                typeof(DescriptionAttribute), false), (
                                    f, a) => new { Field = f, Att = a })
                            .Where(a => ((DescriptionAttribute)a.Att)
                                .Description == description).SingleOrDefault();
            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
