using FocalPtMbl.MainMenu.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.Payments.ViewModels
{
    public class PaymentHistoryDetail
    {
        public string Header { get; set; }

       public ObservableCollection<Payment> PaymentHistory { get; set; }
    }

    public class PaymentViewModel : ThemeBaseViewModel
    {
        public PaymentViewModel(Order order) : base("Payments")
        {
            Order = order;
            PaymentHistory = new PaymentHistoryDetail();
            PaymentHistory.Header = "Payment History";
            DepositPaymentHistory = new PaymentHistoryDetail();
            DepositPaymentHistory.Header = "Deposits & Security Deposits";
            PaymentTypeSelection = new Command<int>((paymentType) => SetPaymentSelectionType(paymentType));
            SetPaymentData();
        }

        public PaymentViewModel()
        {
            PaymentHistory = new PaymentHistoryDetail();
            PaymentHistory.Header = "Payment History";
            DepositPaymentHistory = new PaymentHistoryDetail();
            DepositPaymentHistory.Header = "Deposits & Security Deposits";
            PaymentTypeSelection = new Command<int>((paymentType) => SetPaymentSelectionType(paymentType));
            SetPaymentData();
        }

        private void SetPaymentData()
        {
            if(Order?.Payments?.Count > 0)
            {
                PaymentHistory.PaymentHistory = new ObservableCollection<Payment>(Order.Payments.Where(p => !p.PaymentVoid && !p.PaymentSD && !p.PaymentDeposit));
                DepositPaymentHistory.PaymentHistory = new ObservableCollection<Payment>(Order.Payments.Where(p => !p.PaymentVoid && p.PaymentSD || p.PaymentDeposit));
            }
        }

        public Order Order { get; }

        private PaymentHistoryDetail _paymentHistory;
        public PaymentHistoryDetail PaymentHistory
        {
            get => _paymentHistory;
            set
            {
                _paymentHistory = value;
                OnPropertyChanged(nameof(PaymentHistory));
            }
        }

        private PaymentHistoryDetail _depositPaymentHistory;
        public PaymentHistoryDetail DepositPaymentHistory
        {
            get => _depositPaymentHistory;
            set
            {
                _depositPaymentHistory = value;
                OnPropertyChanged(nameof(DepositPaymentHistory));
            }
        }

        public ICommand PaymentTypeSelection { get; }
        public void SetPaymentSelectionType(int paymentType)
        {

        }
    }
}
