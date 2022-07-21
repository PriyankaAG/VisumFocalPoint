using System.Collections.ObjectModel;
using System.Windows.Input;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.Payments.Types
{
    public class CreditCardPaymentDetails
    {
        public ICommand CardDetailSelectCommand { get; }

        public string Header { get; set; }

        public ObservableCollection<PaymentInfo> CreditCardDetailList { get; set; }
    }
}
