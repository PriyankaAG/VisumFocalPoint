using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FocalPoint.Components.Interface;
using FocalPoint.MainMenu.Views;
using FocalPoint.Modules.Payments.Entity;
using Newtonsoft.Json;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Components.EntityComponents
{
    public class PaymentEntityComponent : IPaymentEntityComponent
    {
        IAPICompnent apiComponent;

        const string PaymentSettings = "Payment/Settings";
        const string PaymentTypes = "Payment/Types/{0}";
        const string PaymentCardInfo = "Payment/CardInfo/{0}";
        const string PaymentACHInfo = "Payment/ACHInfo/{0}";
        const string PaymentProcess = "Payment/Process";
        const string PaymentEmail = "Payment/Email/{0}/{1}";

        public PaymentEntityComponent()
        {
            apiComponent = new APIComponent();
            //apiComponent.AddStoreToHeader("1");
            //apiComponent.AddTerminalToHeader("1");
        }

        public async Task<PaymentSettings> GetPaymentSettings()
        {
            PaymentSettings settings;
            try
            {
                settings = await apiComponent.GetAsync<PaymentSettings>(PaymentSettings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return settings;
        }

        public async Task<List<PaymentType>> GetPaymentTypes(int paymentType)
        {
            List<PaymentType> type;
            try
            {
                type = await apiComponent.GetAsync<List<PaymentType>>(string.Format(PaymentTypes, paymentType));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return type;
        }

        public async Task<List<PaymentInfo>> GetPaymentCardInfo(int customerNo)
        {
            List<PaymentInfo> info;
            try
            {
                info = await apiComponent.GetAsync<List<PaymentInfo>>(string.Format(PaymentCardInfo, customerNo));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return info;
        }

        public async Task<PaymentInfo> GetPaymentACHInfo(int customerNo)
        {
            PaymentInfo info;
            try
            {
                info = await apiComponent.GetAsync<PaymentInfo>(string.Format(PaymentACHInfo, customerNo));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return info;
        }

        public async Task<HttpResponseMessage> PostPaymentProcess(PaymentRequest request)
        {
            HttpResponseMessage result;
            try
            {
                string requestContent = JsonConvert.SerializeObject(new { Request = request });
                result = await apiComponent.PostAsync(PaymentProcess, requestContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<HttpResponseMessage> PaymentUpdate(Payment payment, bool paymentVoid)
        {
            HttpResponseMessage result;
            try
            {
                var serRequest = JsonConvert.SerializeObject(new VoidRequest { Pay = payment, PayVoid = true });
                result = await apiComponent.SendAsync("Payment", serRequest, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<HttpResponseMessage> PaymentEmailPost(string emailAddress, int paymentNo)
        {
            HttpResponseMessage result;
            try
            {

                var serRequest = JsonConvert.SerializeObject(new { EmailAddress = emailAddress, PaymentNo = paymentNo });
                result = await apiComponent.PostAsync(string.Format(PaymentEmail, emailAddress,paymentNo), serRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public void HandleTokenExpired()
        {
            apiComponent.HandleTokenExpired();
        }
    }
}
