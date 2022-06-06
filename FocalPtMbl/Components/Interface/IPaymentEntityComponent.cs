using System.Collections.Generic;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Components.Interface
{
    public interface IPaymentEntityComponent
    {
        Task<PaymentSettings> GetPaymentSettings();
        Task<List<PaymentType>> GetPaymentTypes(int paymentType);
        Task<PaymentInfo> GetPaymentCardInfo(int customerNo);
        Task<PaymentInfo> GetPaymentACHInfo(int customerNo);
        Task<PaymentResponse> PostPaymentProcess(PaymentRequest request);
        Task<bool> PostPaymentEmail(string emailAddress, int paymentNo);
        Task<Payment> PaymentUpdate(Payment pay, bool paymentVoid);
    }
}
