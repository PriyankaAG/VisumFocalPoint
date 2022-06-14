using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FocalPoint.Components.Interface;
using FocalPtMbl.MainMenu.ViewModels;
using MvvmHelpers.Commands;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.Payments.Types
{
    public class CreditCard : ThemeBaseViewModel
    {
        Order order;

        public IPaymentEntityComponent PaymentEntityComponent;
        public CreditCard(Order order, IPaymentEntityComponent paymentEntityComponent)
        {
            this.order = order;
            PaymentEntityComponent = paymentEntityComponent;
            ProcessOnline = true; //defaults to true od Credit card POS
            Task.Run(async () =>
            {
                if (order != null)
                {
                    await GetCreditCardList();
                }
            });
            CreditCardPaymentDetails = new CreditCardPaymentDetails
            {
                Header = "Card on File",
                CreditCardDetailList = new ObservableCollection<PaymentInfo>()
            };
            CardDetailSelectCommand = new Command<PaymentInfo>((PaymentInfo paymentInfo) => UpdateDetails(paymentInfo));
        }
        #region Properties
        public string CardHolderName { get; set; }
        public string CardLast4Digits { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string AuthorizationCode { get; set; }
        public string AvsStreetAddress { get; set; }
        public string AvsZipCode { get; set; }
        public bool StoreCardOnFileEnabled
        {
            get { return Settings == null ? false : Settings.CardOnFile; }
        }
        public bool StoreCardOnFile { get; set; }
        public bool IsCardOnFile
        {
            get
            {
                return Settings == null ? ProcessOnline : Settings.CardOnFile && ProcessOnline;
            }
        }
        private bool processOnline;
        public bool ProcessOnline
        {
            get { return processOnline; }
            set
            {
                processOnline = value;
                OnPropertyChanged(nameof(ProcessOnline));
                OnPropertyChanged(nameof(IsCardOnFile));
            }
        }
        public bool IsStoredCardSelected { get; set; }
        public PaymentSettings Settings { get; set; }
        #endregion
        public ICommand CardDetailSelectCommand { get; }


        private CreditCardPaymentDetails _creditCardPaymentDetails;
        public CreditCardPaymentDetails CreditCardPaymentDetails
        {
            get => _creditCardPaymentDetails;
            set
            {
                _creditCardPaymentDetails = value;
                OnPropertyChanged(nameof(CreditCardPaymentDetails));
            }
        }
        public async Task GetCreditCardList()
        {
            CreditCardPaymentDetails.CreditCardDetailList.Clear();
            List<PaymentInfo> paymentInfos = await PaymentEntityComponent.GetPaymentCardInfo(order.OrderCustNo);
            if (paymentInfos?.Count > 0)
            {
                foreach (PaymentInfo payment in paymentInfos)
                {
                    CreditCardPaymentDetails.CreditCardDetailList.Add(payment);
                }
            }
        }

        private void UpdateDetails(PaymentInfo paymentInfo)
        {
            CardHolderName = paymentInfo.InfoHolder;
            CardLast4Digits = paymentInfo.InfoText;
            if (DateTime.TryParseExact(paymentInfo.InfoExpireDte, "MMddyy", CultureInfo.InvariantCulture,
                           DateTimeStyles.None, out DateTime date))
                ExpirationDate = date;
            ProcessOnline = true;
            IsStoredCardSelected = true;
            OnPropertyChanged(nameof(CardHolderName));
            OnPropertyChanged(nameof(CardLast4Digits));
            OnPropertyChanged(nameof(ExpirationDate));
            OnPropertyChanged(nameof(ProcessOnline));
            OnPropertyChanged(nameof(IsStoredCardSelected));
        }

        public string ValidateCreditCardDetails()
        {
            if (string.IsNullOrEmpty(CardHolderName))
                return "Please enter Credit Card Holder Name";
            else if (CardLast4Digits.Length < 4)
                return "Please enter the last 4 of the Credit Card Number";
            else if (DateTime.Compare(ExpirationDate, DateTime.Now) < 0)
                return "Credit Card Expired!";
            else if (!ProcessOnline && string.IsNullOrEmpty(AuthorizationCode))
                return "Please enter Authorization Code";
            else if (ProcessOnline)
            {
                if (string.IsNullOrEmpty(AvsStreetAddress))
                    return "Please enter Street Address";
                else if (string.IsNullOrEmpty(AvsZipCode))
                    return "Please enter Zip code";
            }
            return "";
        }

        public PaymentRequestCard GetCardDetails()
        {
            return new PaymentRequestCard
            {
                CardHolder = CardHolderName,
                LastFour = CardLast4Digits,
                Expiration = ExpirationDate.ToString(),
                AuthCode = ProcessOnline ? null : AuthorizationCode,
                OnLine = ProcessOnline,
                Street = ProcessOnline ? AvsStreetAddress : null,
                Zipcode = ProcessOnline ? AvsZipCode : null,
                StoreInfo = StoreCardOnFile,
                ManualToken = null
            };
        }
        public void ResetCreditCard()
        {
            CardHolderName = CardLast4Digits = AuthorizationCode = AvsStreetAddress = AvsZipCode = "";
            StoreCardOnFile = false;
            ExpirationDate = DateTime.Now;
            IsStoredCardSelected = false;
            OnPropertyChanged(nameof(CardHolderName));
            OnPropertyChanged(nameof(CardLast4Digits));
            OnPropertyChanged(nameof(AuthorizationCode));
            OnPropertyChanged(nameof(AvsStreetAddress));
            OnPropertyChanged(nameof(AvsZipCode));
            OnPropertyChanged(nameof(StoreCardOnFile));
            OnPropertyChanged(nameof(ExpirationDate));
            OnPropertyChanged(nameof(IsStoredCardSelected));
        }
    }

    public class PaymentHistoryDetail
    {
        public string Header { get; set; }

        public ObservableCollection<Payment> PaymentHistory { get; set; }
    }

    public class CreditCardPaymentDetails
    {
        public ICommand CardDetailSelectCommand { get; }

        public string Header { get; set; }

        public ObservableCollection<PaymentInfo> CreditCardDetailList { get; set; }
    }
}
