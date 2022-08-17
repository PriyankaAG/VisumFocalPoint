using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Components.Interface
{
    public interface IPaymentEntityComponent
    {
        Task<PaymentSettings> GetPaymentSettings();
        Task<List<PaymentType>> GetPaymentTypes(int paymentType);
        Task<List<PaymentInfo>> GetPaymentCardInfo(int customerNo);
        Task<PaymentInfo> GetPaymentACHInfo(int customerNo);
        Task<HttpResponseMessage> PostPaymentProcess(PaymentRequest request);
        Task<HttpResponseMessage> PaymentEmailPost(string emailAddress, int paymentNo);
        Task<HttpResponseMessage> PaymentUpdate(Payment pay, bool paymentVoid);
        void HandleTokenExpired();
    }
}
