using FocalPoint.Data;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{
    public class QuickOrderHeaderViewModel : ThemeBaseViewModel
    {
        public QuickOrderHeaderViewModel()
        {
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
        }

        private DateTime startdte = DateTime.Now;
        private DateTime enddte = DateTime.Now.AddDays(1);
        public DateTime StartDate
        {
            get { return startdte; }
            set
            {
                if (startdte != value)
                {
                    startdte = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }
        public DateTime EndDate
        {
            get { return enddte; }
            set
            {
                if (enddte != value)
                {
                    enddte = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }
        private DateTime startTime = DateTime.Now;
        private DateTime endTime = DateTime.Now.AddDays(1);
        public DateTime StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    OnPropertyChanged(nameof(StartTime));
                }
            }
        }
        public DateTime EndTime
        {
            get { return endTime; }
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    OnPropertyChanged(nameof(EndTime));
                }
            }
        }


        HttpClient clientHttp;
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        OrderUpdate orderUpdate;

        private ObservableCollection<string> states = new ObservableCollection<string>();
        public ObservableCollection<string> States
        {
            get { return states; }
            set
            {
                if (states != value)
                {
                    states = value;
                }
            }
        }
        private ObservableCollection<string> lengths = new ObservableCollection<string>();
        public ObservableCollection<string> Lengths
        {
            get { return lengths; }
            set
            {
                if (lengths != value)
                {
                    lengths = value;
                }
            }
        }
        public OrderSettings OrderSettings = new OrderSettings();
        internal void GetOrderSettings()
        {
            try
            {
                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "OrderSettings/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));

                var response = ClientHTTP.GetAsync(uri).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    var orderSettings = JsonConvert.DeserializeObject<OrderSettings>(content);
                    OrderSettings = orderSettings;
                    foreach (var length in orderSettings.Lengths)
                    {
                        Lengths.Add(length.Display);
                    }
                }
            }catch(Exception ex)
            {

            }
            }
        internal void GetStates()
        {
            CustomerSettings customerSettings = new CustomerSettings();
            try
            {
                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "CustomerSettings/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));

                var response = ClientHTTP.GetAsync(uri).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                     customerSettings = JsonConvert.DeserializeObject<CustomerSettings>(content);
                }
                Uri uri2 = new Uri(string.Format(DataManager.Settings.ApiUri + "States/" + customerSettings.Defaults.CustomerCountry.ToString()));

                var response2 = ClientHTTP.GetAsync(uri2).GetAwaiter().GetResult();
                if (response2.IsSuccessStatusCode)
                {
                    string content2 = response2.Content.ReadAsStringAsync().Result;
                    List<DisplayValueString> statesAPI = JsonConvert.DeserializeObject<List<DisplayValueString>>(content2);
                    foreach (var state in statesAPI)
                    {
                        States.Add(state.Value);
                        // StatesAPI.Add(state);
                    }
                }
                else
                    States = new ObservableCollection<string>();

            }
            catch (Exception ex)
            {
                States = new ObservableCollection<string>();
            }
}

        internal void GetEndDateAndTimeValues(string selectedDateTimeValue)
        {
            foreach (var displayValue in OrderSettings.Lengths)
            {
                if (displayValue.Display == selectedDateTimeValue)
                {
                   var selValue = displayValue.Value;
                    GetEndDateTime(selValue);
                }
            }
        }
        private void GetEndDateTime(string dateTimeValue)
        {
            DateTime newEndDte = new DateTime();
            switch(dateTimeValue)
            {
                case "1H":
                    EndDate = StartDate.AddHours(1);
                    break;
                case "2H":
                    EndDate = StartDate.AddHours(2);
                    break;
                case "3H":
                    EndDate = StartDate.AddHours(3);
                    break;
                case "4H":
                    EndDate = StartDate.AddHours(4);
                    break;
                case "5H":
                    EndDate = StartDate.AddHours(5);
                    break;
                case "6H":
                    EndDate = StartDate.AddHours(6);
                    break;
                case "ON":
                    EndDate = StartDate.AddHours(0);
                    break;
                case "OC":
                    EndDate = StartDate.AddHours(0);
                    break;
                case "1D":
                    EndDate = StartDate.AddDays(1);
                    break;
                case "2D":
                    EndDate = StartDate.AddDays(2);
                    break;
                case "3D":
                    EndDate = StartDate.AddDays(3);
                    break;
                case "4D":
                    EndDate = StartDate.AddDays(4);
                    break;
                case "5D":
                    EndDate = StartDate.AddDays(5);
                    break;
                case "6D":
                    EndDate = StartDate.AddDays(6);
                    break;
                case "WK":
                    EndDate = StartDate.AddDays(3);
                    break;
                case "W2":
                    EndDate = StartDate.AddDays(4);
                    break;
                case "1W":
                    EndDate = StartDate.AddDays(7);
                    break;
                case "2W":
                    EndDate = StartDate.AddDays(14);
                    break;
                case "3W":
                    EndDate = StartDate.AddDays(21);
                    break;
                case "1M":
                    EndDate = StartDate.AddMonths(1);
                    break;
                case "E1":
                    EndDate = StartDate.AddHours(0);
                    break;
                case "S1":
                    EndDate = StartDate.AddHours(0);
                    break;
                case "S2":
                    EndDate = StartDate.AddHours(0);
                    break;
                case "S3":
                    EndDate = StartDate.AddHours(0);
                    break;
                case "S4":
                    EndDate = StartDate.AddHours(0);
                    break;
                case "S5":
                    EndDate = StartDate.AddHours(0);
                    break;
                case "S6":
                    EndDate = StartDate.AddHours(0);
                    break;
                case "S7":
                    EndDate = StartDate.AddHours(0);
                    break;
                case "OE":
                    EndDate = StartDate.AddHours(0);
                    break;
                default:
                    EndDate.AddHours(0);
                    break;

            }
        }

        internal OrderUpdate ChangeHeadder(DateTime startDate, DateTime EndDate, Order curOrder, OrderUpdate myOrderUpdate, out QuestionFaultExceptiom result)
        {
            // success no questions needed
            result = null;
            try
            {
                var Update = new OrderUpdate();
                orderUpdate = new OrderUpdate();
                //update searchText
                // KIRK REM
                //foreach (var kvp in myOrderUpdate.Answers)
                //    myOrderUpdate.Questions.Add(new QuestionAnswer { Answer = kvp.Value, Code = kvp.Key });

                //foreach (var listItem in myOrderUpdate.Questions)
                //    myOrderUpdate.Answers[listItem.Code] = listItem.Answer;
                // END KIRK
                //MerchItem.Serials
                OrderUpdate OrderToUpDate = new OrderUpdate();

                Uri uriUpdateCust = new Uri(string.Format(DataManager.Settings.ApiUri + "Order/"));
                var stringContent = new StringContent(
                    JsonConvert.SerializeObject(new { Update }),
                      Encoding.UTF8,
                      "application/json");
                var responseOrderUpdate = ClientHTTP.PutAsync(uriUpdateCust, stringContent).GetAwaiter().GetResult();
                if (responseOrderUpdate.IsSuccessStatusCode)
                {
                    string orderContent = responseOrderUpdate.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };

                    orderUpdate = JsonConvert.DeserializeObject<OrderUpdate>(orderContent, settings);
                    orderUpdate.Answers.Clear();
                    return orderUpdate;
                }
                if (responseOrderUpdate.StatusCode == System.Net.HttpStatusCode.ExpectationFailed)
                {
                   // previousItem = RentalItem;
                    string readErrorContent = responseOrderUpdate.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };

                    result = JsonConvert.DeserializeObject<QuestionFaultExceptiom>(readErrorContent, settings);
                }
                return orderUpdate;
            }
            catch (Exception ex)
            {
                result = null;
                return null;
            }
        }
    }
}
