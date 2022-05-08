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
    public class QuickOrderPaymentsSelectViewModel : ThemeBaseViewModel
    {
        ObservableCollection<PayTypeAndImage> recent;
        public ObservableCollection<PayTypeAndImage> Recent
        {
            get => this.recent;
            private set
            {
                this.recent = value;
                OnPropertyChanged(nameof(Recent));
            }
        }
        private ObservableCollection<string> currentPaymentTypes;
        public ObservableCollection<string> CurrentPaymentTypes
        {
            get => this.currentPaymentTypes;
            private set
            {
                this.currentPaymentTypes = value;
                OnPropertyChanged(nameof(CurrentPaymentTypes));
            }
        }
        public QuickOrderPaymentsSelectViewModel()
        {

            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
            currentPaymentTypes = new ObservableCollection<string>();
            // clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
        }


        HttpClient clientHttp;
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }

        internal void GetPaymentOptions()
        {
            try
            {//0 for new payment
                string RequestType = "0";
                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "Payment/Types/"+ RequestType));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                var response = ClientHTTP.GetAsync(uri).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    var paymentTypes = JsonConvert.DeserializeObject<List<PaymentType>>(content);
                    if (recent == null)
                    {
                        Recent = new ObservableCollection<PayTypeAndImage>();
                        foreach (var payment in paymentTypes)
                        {
                            Recent.Add(new PayTypeAndImage(payment));
                            CurrentPaymentTypes.Add(payment.PaymentDscr);
                        }
                    }
                    else
                    {
                        Recent.Clear();
                        foreach (var payment in paymentTypes)
                        {
                            Recent.Add(new PayTypeAndImage(payment));
                            CurrentPaymentTypes.Add(payment.PaymentDscr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
    public class PayTypeAndImage
    {
        public string ImageName { get; set; }
        public string PaymentName { get; set; }
        public PayTypeAndImage(PaymentType paymentType)
        {
            PaymentName = paymentType.PaymentDscr;
            ImageName = LookUpImageName(paymentType.PaymentTIcon);
        }

        private string LookUpImageName(byte paymentTIcon)
        {
            string paymentIconName = "";
            switch (paymentTIcon)
            {
                case 0:
                    paymentIconName = "Blank_96.png";
                    break;
                case 1:
                    paymentIconName = "american_express_96.png";
                    break;
                case 2:
                    paymentIconName = "cash_96.png";
                    break;
                case 3:
                    paymentIconName = "check_96.png";
                    break;
                case 4:
                    paymentIconName = "Coupon_96.png";
                    break;
                case 5:
                    paymentIconName = "credit_cards_96.png";
                    break;
                case 6:
                    paymentIconName = "cirrus_96.png";
                    break;
                case 7:
                    paymentIconName = "diners_club_96.png";
                    break;
                case 8:
                    paymentIconName = "discover_96.png";
                    break;
                case 9:
                    paymentIconName = "e_check_96.png";
                    break;
                case 10:
                    paymentIconName = "gift_card_96.png";
                    break;
                case 11:
                    paymentIconName = "gift_card_96.png";
                    break;
                case 12:
                    paymentIconName = "mastercard_96.png";
                    break;
                case 13:
                    paymentIconName = "paypal_96.png";
                    break;
                case 14:
                    paymentIconName = "purchase_order_96.png";
                    break;
                case 15:
                    paymentIconName = "vis_96.png";
                    break;
                default:
                    paymentIconName = "Blank_96.png";
                    break;

            }
            return paymentIconName;
        }
    }
}
