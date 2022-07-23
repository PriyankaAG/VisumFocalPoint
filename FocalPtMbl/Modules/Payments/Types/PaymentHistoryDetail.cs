using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using FocalPoint.Components.Interface;
using FocalPoint.Modules.Payments.Entity;
using Newtonsoft.Json;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.Payments.Types
{
    public class PaymentHistoryDetail : CommonViewModel
    {
        public IPaymentEntityComponent PaymentEntityComponent;
        public string Header { get; set; }
        public bool ShowVoid { get; set; }
        public ICommand VoidPaymentCommand { get; }
        public ObservableCollection<Payment> PaymentHistory { get; set; }

        public PaymentHistoryDetail(IPaymentEntityComponent paymentEntityComponent)
        {
            PaymentEntityComponent = paymentEntityComponent;
            VoidPaymentCommand = new MvvmHelpers.Commands.Command<Payment>(payment => VoidPayment(payment));
        }

        private async void VoidPayment(Payment payment)
        {
            try
            {
                Indicator = true;
                payment.PaymentVoid = true;
                var httpResponseMessage = await PaymentEntityComponent.PaymentUpdate(payment, false);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string content = await httpResponseMessage.Content.ReadAsStringAsync();
                    var voidResponse = JsonConvert.DeserializeObject<VoidResponse>(content);
                    if (voidResponse != null)
                    {
                        if (voidResponse.Notifications != null && voidResponse.Notifications.Any())
                        {
                            foreach (var item in voidResponse.Notifications)
                            {
                                await Application.Current.MainPage.DisplayAlert("Focal Point", item, "Ok");
                            }
                        }
                        else if (voidResponse.Payment != null)
                        {
                            PaymentHistory.Remove(payment);
                        }
                    }
                }
                else
                {
                    string contentStr = await httpResponseMessage.Content.ReadAsStringAsync();
                    await Application.Current.MainPage.DisplayAlert("Focal Point", contentStr, "Ok");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Focal Point", ex.Message, "Ok");
            }
            finally
            {
                Indicator = false;
            }
        }
    }
}
