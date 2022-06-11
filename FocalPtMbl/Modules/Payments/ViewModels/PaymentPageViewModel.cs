using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Visum.Services.Mobile.Entities;
using static Visum.Services.Mobile.Entities.PaymentRequest;

namespace FocalPoint.Modules.Payments.ViewModels
{
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

    public class PaymentPageViewModel : CommonViewModel
    {
        IGeneralComponent generalComponent;
        public Order Order { get; }
        public string AmountDue { get { return Order?.Totals?.TotalDueAmt.ToString("c"); } }
        public string SuggestedDeposit { get { return Order?.Totals?.TotalSugDepositAmt.ToString("c"); } }
        private PaymentSettings settings;
        public IPaymentEntityComponent PaymentEntityComponent { get; set; }
        public RequestTypes RequestType { get; set; }
        public List<string> DepositTypes { get; set; }
        private string paymentMethod;
        public string PaymentMethod
        {
            get { return paymentMethod; }
            set
            {
                paymentMethod = value;
                OnPropertyChanged(nameof(PaymentMethod));
            }
        }
        public string DepositType { get; set; }
        public string PaymnetType { get; set; }
        #region Other
        public string OtherNumber { get; set; }
        #endregion

        #region Check Payment
        public string CheckNumber { get; set; }
        public string LicenseNumber { get; set; }
        public List<State> LicenseStates { get; set; }
        public State SelectedLicenseState { get; set; }
        #endregion

        #region CreditCard
        public string CardHolderName { get; set; }
        public string CardLast4Digits { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string AuthorizationCode { get; set; }
        public string AvsStreetAddress { get; set; }
        public string AvsZipCode { get; set; }
        public bool StoreCardOnFileEnabled
        {
            get { return settings == null ? false : settings.CardOnFile; }
        }
        public bool StoreCardOnFile { get; set; }
        public bool IsCardOnFile
        {
            get
            {
                return settings == null ? ProcessOnline : settings.CardOnFile && ProcessOnline;
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
        #endregion

        public string Payment { get; set; }
        public string TotalReceived { get; set; }
        public string ChangeDue { get; set; }
        public bool IsOtherType { get; set; }
        public bool IsCash { get; set; }
        public bool IsCheck { get; set; }
        public bool IsCreditCard { get; set; }
        public bool IsCreditCardPOS { get; set; }
        public bool ShowDueAndReceived { get; set; }

        public string OtherTitle { get; set; }
        public List<PaymentType> PaymentTypes { get; set; }
        private PaymentType selectedPaymentType;
        public PaymentType SelectedPaymentType
        {
            get => selectedPaymentType;
            set
            {
                selectedPaymentType = value;
                SetCardView(selectedPaymentType);
            }
        }
        public List<string> CashPayments
        {
            get { return getCashPayments(); }
        }
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

        public ICommand PaymentTypeSelection { get; }
        public ICommand CardDetailSelectCommand { get; }

        #region const
        public PaymentPageViewModel(Order order) : base("Payments")
        {
            generalComponent = new GeneralComponent();
            PaymentEntityComponent = new PaymentEntityComponent();
            PaymentTypeSelection = new Command<int>((paymentType) => SetPaymentSelectionType(paymentType));
            CardDetailSelectCommand = new Command<PaymentInfo>((PaymentInfo paymentInfo) => UpdateDetails(paymentInfo));
            GetSettings().ContinueWith((a) => { settings = a.Result; });
            Order = order;
            PaymentHistory = new PaymentHistoryDetail();
            PaymentHistory.Header = "Payment History";
            DepositPaymentHistory = new PaymentHistoryDetail();
            DepositPaymentHistory.Header = "Deposits & Security Deposits";
            CreditCardPaymentDetails = new CreditCardPaymentDetails();
            CreditCardPaymentDetails.Header = "Card on File";
            CreditCardPaymentDetails.CreditCardDetailList = new ObservableCollection<PaymentInfo>();
            SetPaymentData();
            ProcessOnline = true;
            if (Order?.Customer != null)
                GetLicenseStates(Order.Customer.CustomerCountry);
            Task.Run(async () =>
            {
                if (Order != null)
                {
                    await GetCreditCardList();
                }
            });
        }

        private void UpdateDetails(PaymentInfo paymentInfo)
        {
        }

        #endregion

        private void GetLicenseStates(int countryCode)
        {
            try
            {
                generalComponent.GetStates(countryCode).ContinueWith(task =>
                {
                    LicenseStates = task.Result.ToList();
                });
            }
            catch
            { }
        }
        private static List<string> getCashPayments()
        {
            var list = new List<string>();
            list.Add("$1");
            list.Add("$2");
            for (int i = 1; i <= 20; i++)
            {
                var num = 5 * i;
                list.Add("$" + num + "");
            }
            return list;
        }
        public async void SetPaymentSelectionType(int paymentType)
        {

        }
        private async Task<PaymentSettings> GetSettings()
        {
            return await PaymentEntityComponent.GetPaymentSettings();
        }

        public async Task SetPaymenyTypes(RequestTypes requestType)
        {
            RequestType = requestType;
            List<PaymentType> lstPaymentTypes = await PaymentEntityComponent.GetPaymentTypes((int)requestType);
            if (lstPaymentTypes != null && lstPaymentTypes.Any())
            {
                PaymentTypes = lstPaymentTypes;
                OnPropertyChanged(nameof(PaymentTypes));
            }
        }

        private async Task GetCreditCardList()
        {
            CreditCardPaymentDetails.CreditCardDetailList.Clear();
            List<PaymentInfo> paymentInfos = await PaymentEntityComponent.GetPaymentCardInfo(Order.OrderCustNo);
            if (paymentInfos?.Count() > 0)
            {
                foreach (PaymentInfo payment in paymentInfos)
                {
                    CreditCardPaymentDetails.CreditCardDetailList.Add(payment);
                }
            }
        }

        internal void SetCardView(PaymentType paymentType)
        {
            IsCash = IsCheck = IsCreditCard = IsOtherType = IsCreditCardPOS = false;
            ShowDueAndReceived = true;

            switch (paymentType.PaymentKind)
            {
                case "CA":
                    IsCash = true;
                    break;
                case "CK":
                    IsCheck = true;
                    break;
                case "CC":
                    if (settings != null && settings.POSEnabled)
                    {
                        IsCreditCardPOS = true;
                    }
                    else
                    {
                        IsCreditCard = true;
                    }
                    ShowDueAndReceived = false;
                    break;
                case "CP":
                case "MS":
                case "IR":
                case "OT":
                case "DS":
                    OtherTitle = paymentType.PaymentDscr;
                    IsOtherType = true;
                    ShowDueAndReceived = false;
                    break;
                    //case "DC":
                    //    IsDebitCard = true;
                    //    break;
            }
            OnPropertyChanged(nameof(IsCash));
            OnPropertyChanged(nameof(IsCheck));
            OnPropertyChanged(nameof(IsCreditCard));
            OnPropertyChanged(nameof(IsOtherType));
            OnPropertyChanged(nameof(IsCreditCardPOS));
            OnPropertyChanged(nameof(ShowDueAndReceived));
            OnPropertyChanged(nameof(OtherTitle));
            OnPropertyChanged(nameof(IsCardOnFile));

            /*var otherTypes = new string[] { "CP", "MS", "IR", "OT", "DS" };
            IsOtherType = otherTypes.Contains(selectedItem);
            IsCheck = selectedItem == "CK";
            IsCreditCard = selectedItem == "CC";*/
        }
        public string GetPaymentImage(byte paymentTIcon)
        {
            return paymentTIcon switch
            {
                0 => "Blank_96.png",
                1 => "american_express_96.png",
                2 => "cash_96.png",
                3 => "check_96.png",
                4 => "Coupon_96.png",
                5 => "credit_cards_96.png",
                6 => "cirrus_96.png",
                7 => "diners_club_96.png",
                8 => "discover_96.png",
                9 => "e_check_96.png",
                10 => "gift_card_96.png",
                11 => "gift_card_2_96.png",
                12 => "mastercard_96.png",
                13 => "paypal_96.png",
                14 => "purchase_order_96.png",
                15 => "visa_96.png",
                16 => "loyalty_96.png",
                17 => "discount_96.png",
                18 => "internal_refund_96.png",
                _ => "Blank_96.png",
            };
        }
        private void SetPaymentData()
        {
            if (Order?.Payments?.Count > 0)
            {
                PaymentHistory.PaymentHistory = new ObservableCollection<Payment>(Order.Payments.Where(p => !p.PaymentVoid && !p.PaymentSD && !p.PaymentDeposit));
                DepositPaymentHistory.PaymentHistory = new ObservableCollection<Payment>(Order.Payments.Where(p => !p.PaymentVoid && p.PaymentSD || p.PaymentDeposit));
            }
        }
        internal void SetPayment(decimal value)
        {
            Payment = 0.0.ToString("c");
            TotalReceived = ChangeDue = value.ToString("c");
            OnPropertyChanged(nameof(TotalReceived));
            OnPropertyChanged(nameof(ChangeDue));
            OnPropertyChanged(nameof(Payment));
        }
        internal string ValidatePaymentKinds()
        {
            var validationMessage = "";
            switch (SelectedPaymentType.PaymentKind)
            {
                /*case "CP":
                case "MS":
                case "IR":
                case "OT":
                case "DS":
                    if (string.IsNullOrEmpty(OtherNumber))
                        validationMessage = "Please enter " + SelectedPaymentType.PaymentDscr + " Number";
                    break;*/
                case "CK":
                    validationMessage = ValidateCheckDetails();
                    break;
                case "CC":
                    validationMessage = ValidateCreditCardDetails();
                    break;
            }
            if (string.IsNullOrEmpty(validationMessage))
                validationMessage = ValidatePayment();
            return validationMessage;
        }
        private string ValidateCreditCardDetails()
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
        private string ValidateCheckDetails()
        {
            var message = "";
            if (string.IsNullOrEmpty(CheckNumber))
                message = "Check Number is empty";
            else if (string.IsNullOrEmpty(SelectedLicenseState.Display))
                message = "Must select a LicenseState";
            return message;
        }
        private string ValidatePayment()
        {
            if (SelectedPaymentType.PaymentKind == "CA" || SelectedPaymentType.PaymentKind == "CK")
            {
                if (decimal.TryParse(TotalReceived.Trim('$'), out decimal total) && total == 0)
                    return "Cannot except a Zero Dollar Payment!";
            }
            else if (decimal.TryParse(Payment.Trim('$'), out decimal payment) && payment == 0)
                return "Cannot except a Zero Dollar Payment!";
            return "";
        }
        internal async Task<PaymentResponse> ProcessPayment()
        {
            var request = new PaymentRequest
            {
                RequestType = RequestType,
                Source = "FC",
                SourceID = Order?.OrderNo ?? -1,
                CustomerNo = Order?.OrderCustNo ?? -1,
                PaymentTNo = SelectedPaymentType.PaymentTNo,
                PaymentAmt = decimal.TryParse(TotalReceived.Trim('$'), out decimal total) ? total : 0,
                CashBackAmt = decimal.TryParse(ChangeDue.Trim('$'), out decimal due) ? due : 0,
                TaxAmt = Order?.OrderTax ?? -1,
                OnFileNo = 0,
                Other = GetOtherDetails(),
                Check = GetCheckDetails(),
                eCheck = null,
                Card = GetCardDetails()
            };
            return await PaymentEntityComponent.PostPaymentProcess(request);
        }
        private PaymentRequestCard GetCardDetails()
        {
            return SelectedPaymentType?.PaymentKind == "CC"
                ? new PaymentRequestCard
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
                }
                : null;
        }
        private PaymentRequestCheck GetCheckDetails()
        {
            return SelectedPaymentType?.PaymentKind == "CK"
                ? new PaymentRequestCheck
                {
                    Number = CheckNumber,
                    DLNumber = LicenseNumber,
                    DLState = SelectedLicenseState.Value
                }
                : null;
        }
        private PaymentRequestOther GetOtherDetails()
        {
            var payKind = SelectedPaymentType.PaymentKind;
            return payKind == "CP" || payKind == "MS" || payKind == "IR" || payKind == "OT" || payKind == "DS"
                ? new PaymentRequestOther { Number = OtherNumber }
                : null;
        }
        public void ResetCreditCard()
        {
            CardHolderName = CardLast4Digits = AuthorizationCode = AvsStreetAddress = AvsZipCode = "";
            StoreCardOnFile = false;
            ExpirationDate = DateTime.Now;
        }
        public void ResetCheck()
        {
            CheckNumber = LicenseNumber = "";
            if (Order?.Customer != null)
            {
                SelectedLicenseState = LicenseStates?.FirstOrDefault(x => x.Value == Order.Customer.CustomerState);
                OnPropertyChanged(nameof(SelectedLicenseState));
            }
        }
        public void ResetOther()
        {
            OtherNumber = "";
            OnPropertyChanged(nameof(OtherNumber));
        }
    }
}
