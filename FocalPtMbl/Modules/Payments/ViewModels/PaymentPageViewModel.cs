using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using FocalPoint.Modules.Payments.Types;
using FocalPoint.Validations;
using FocalPoint.Validations.Rules;
using FocalPtMbl.MainMenu.ViewModels;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Visum.Services.Mobile.Entities;
using static Visum.Services.Mobile.Entities.PaymentRequest;

namespace FocalPoint.Modules.Payments.ViewModels
{
    public class PaymentPageViewModel : CommonViewModel
    {
        IGeneralComponent generalComponent;
        private IPaymentEntityComponent paymentEntityComponent;
        private ViewOrderEntityComponent orderComponent;


        private Order order;
        public Order Order
        {
            get { return order; }
            set
            {
                order = value;
                OnPropertyChanged(nameof(Order));
                OnPropertyChanged(nameof(AmountDue));
                OnPropertyChanged(nameof(SuggestedDeposit));
            }
        }
        public string AmountDue { get { return Order?.Totals?.TotalDueAmt.ToString("c"); } }
        public string SuggestedDeposit { get { return Order?.Totals?.TotalSugDepositAmt.ToString("c"); } }
        public PaymentSettings Settings;
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
        public ValidatableObject<string> CheckNumber { get; set; }
        public string LicenseNumber { get; set; }
        public List<State> LicenseStates { get; set; }
        public State SelectedLicenseState { get; set; }
        #endregion
        public ValidatableObject<string> Payment { get; set; }
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
                //Payment = RequestType == RequestTypes.Standard ? AmountDue : SuggestedDeposit;
                //OnPropertyChanged(Payment);
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

        private CreditCard _creditCardDetails;
        public CreditCard CreditCardDetails
        {
            get => _creditCardDetails;
            set
            {
                _creditCardDetails = value;
                OnPropertyChanged(nameof(CreditCardDetails));
            }
        }
        public ICommand ValidateCheckNumberCommand => new Command(() => ValidateField(CheckNumber));
        public ICommand ValidatePaymentCommand => new Command(() => ValidateField(Payment));

        #region const
        public PaymentPageViewModel() : base("Payments")
        {
            generalComponent = new GeneralComponent();
            paymentEntityComponent = new PaymentEntityComponent();
            orderComponent = new ViewOrderEntityComponent();
            GetSettings().ContinueWith((a) =>
            {
                Settings = a.Result;
                //Settings.POSEnabled = false;
            });
            PaymentHistory = new PaymentHistoryDetail
            {
                Header = "Payment History"
            };
            DepositPaymentHistory = new PaymentHistoryDetail
            {
                Header = "Deposits & Security Deposits"
            };
            GetOrderDetails();
            //SetPaymentData();
            //if (Order?.Customer != null)
            //    GetLicenseStates(Order.Customer.CustomerCountry);

            CheckNumber = new ValidatableObject<string>();
            Payment = new ValidatableObject<string>();
            AddValidation();
            //SetEntityDetails(DocKinds.Order, Order.OrderNo, "P");
        }

        private void GetOrderDetails()
        {
            Indicator = true;
            orderComponent.GetOrderDetails(501842).ContinueWith(task =>
            {
                Order = task.Result;
                GetLicenseStates(Order.Customer.CustomerCountry);
                SetEntityDetails(DocKinds.Order, Order.OrderNo, "P");
                SetPaymentData();
                Indicator = false;
            });
            OnPropertyChanged(nameof(Order));
        }

        //public PaymentPageViewModel(Order order) : base("Paymnets")
        //{
        //    Order = order;
        //}

        public PaymentPageViewModel(Order order) : base("Payments")
        {
            generalComponent = new GeneralComponent();
            paymentEntityComponent = new PaymentEntityComponent();
            GetSettings().ContinueWith((a) =>
            {
                Settings = a.Result;
                //Settings.POSEnabled = false;
            });
            Order = order;
            PaymentHistory = new PaymentHistoryDetail
            {
                Header = "Payment History"
            };
            DepositPaymentHistory = new PaymentHistoryDetail
            {
                Header = "Deposits & Security Deposits"
            };
            SetPaymentData();
            if (Order?.Customer != null)
                GetLicenseStates(Order.Customer.CustomerCountry);

            CheckNumber = new ValidatableObject<string>();
            Payment = new ValidatableObject<string>();
            AddValidation();
            SetEntityDetails(DocKinds.Order, Order.OrderNo, "P");
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
        private void AddValidation()
        {
            CheckNumber.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Validation failed. Please fill required data." });
            Payment.Validations.Add(new IsValidDecimal<string> { ValidationMessage = "Cannot except a Zero Dollar Payment!" });
        }
        private bool ValidateField(ValidatableObject<string> validatableField)
        {
            return validatableField.Validate();
        }
        private async Task<PaymentSettings> GetSettings()
        {
            return await paymentEntityComponent.GetPaymentSettings();
        }

        public async Task SetPaymenyTypes(RequestTypes requestType)
        {
            RequestType = requestType;
            List<PaymentType> lstPaymentTypes = await paymentEntityComponent.GetPaymentTypes((int)requestType);
            if (lstPaymentTypes != null && lstPaymentTypes.Any())
            {
                PaymentTypes = lstPaymentTypes;
                OnPropertyChanged(nameof(PaymentTypes));
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
                    CreditCardDetails = new CreditCard(Order, paymentEntityComponent, Settings);
                    if (Settings != null && Settings.POSEnabled)
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
            }
            OnPropertyChanged(nameof(IsCash));
            OnPropertyChanged(nameof(IsCheck));
            OnPropertyChanged(nameof(IsCreditCard));
            OnPropertyChanged(nameof(IsOtherType));
            OnPropertyChanged(nameof(IsCreditCardPOS));
            OnPropertyChanged(nameof(ShowDueAndReceived));
            OnPropertyChanged(nameof(OtherTitle));
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
                OnPropertyChanged(nameof(PaymentHistory.PaymentHistory));
            }
        }
        internal void SetSelectedPayment(decimal value)
        {
            Payment.Value = 0.0.ToString("c");
            TotalReceived = ChangeDue = value.ToString("c");
            OnPropertyChanged(nameof(TotalReceived));
            OnPropertyChanged(nameof(ChangeDue));
            OnPropertyChanged(nameof(Payment));
        }

        internal void SetDueAmout()
        {
            if (SelectedPaymentType.PaymentKind == "CA" || SelectedPaymentType.PaymentKind == "CK")
            {
                TotalReceived = ChangeDue = 0.0.ToString("c");
                OnPropertyChanged(nameof(TotalReceived));
                OnPropertyChanged(nameof(ChangeDue));
            }
            Payment.Value = RequestType == RequestTypes.Standard ? AmountDue : SuggestedDeposit;
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
                    validationMessage = _creditCardDetails.ValidateCreditCardDetails();
                    break;
            }
            if (string.IsNullOrEmpty(validationMessage))
                validationMessage = ValidatePayment();
            return validationMessage;
        }
        private string ValidateCheckDetails()
        {
            ValidateField(CheckNumber);
            var message = "";
            if (!CheckNumber.IsValid)
                message = CheckNumber.Errors?.First() ?? "Validation failed";

            else if (string.IsNullOrEmpty(SelectedLicenseState.Display))
                message = "Must select a LicenseState";
            return message;
        }
        private string ValidatePayment()
        {
            ValidateField(Payment);
            if (!Payment.IsValid)
                return Payment.Errors?.First() ?? "Validation failed";
            //if (SelectedPaymentType.PaymentKind == "CA" || SelectedPaymentType.PaymentKind == "CK")
            //{
            //    if (decimal.TryParse(TotalReceived.Trim('$'), out decimal total) && total == 0)
            //        return "Cannot except a Zero Dollar Payment!";
            //}
            //else if (decimal.TryParse(Payment.Value.Trim('$'), out decimal payment) && payment == 0)
            //    return "Cannot except a Zero Dollar Payment!";
            return "";
        }
        internal async Task<PaymentResponse> ProcessPayment()
        {
            try
            {
                var request = new PaymentRequest
                {
                    RequestType = RequestType,
                    Source = "FC",
                    SourceID = Order?.OrderNo ?? -1,
                    CustomerNo = Order?.OrderCustNo ?? -1,
                    PaymentTNo = SelectedPaymentType.PaymentTNo,
                    PaymentAmt = decimal.TryParse(Payment.Value?.Trim('$'), out decimal total) ? total : 0,
                    CashBackAmt = decimal.TryParse(ChangeDue?.Trim('$'), out decimal due) ? due : 0,
                    TaxAmt = Order?.OrderTax ?? -1,
                    OnFileNo = GetOnFileNo(),
                    Other = GetOtherDetails(),
                    Check = GetCheckDetails(),
                    eCheck = null,
                    Card = SelectedPaymentType?.PaymentKind == "CC" ? _creditCardDetails.GetCardDetails() : null
                };
                return await paymentEntityComponent.PostPaymentProcess(request);
            }
            catch
            {
                throw;
            }
        }

        private int GetOnFileNo()
        {
            return SelectedPaymentType?.PaymentKind == "CC" && _creditCardDetails.IsStoredCardSelected
                ? _creditCardDetails.StoredCardInfo.InfoID
                : 0;
        }

        private PaymentRequestCheck GetCheckDetails()
        {
            return SelectedPaymentType?.PaymentKind == "CK"
                ? new PaymentRequestCheck
                {
                    Number = CheckNumber.Value,
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
        private void ResetCheck()
        {
            CheckNumber.Value = LicenseNumber = "";
            CheckNumber.IsValid = true;
            OnPropertyChanged(nameof(CheckNumber));
            OnPropertyChanged(nameof(LicenseNumber));
            if (Order?.Customer != null)
            {
                SelectedLicenseState = LicenseStates?.FirstOrDefault(x => x.Value == Order.Customer.CustomerState);
                OnPropertyChanged(nameof(SelectedLicenseState));
            }
        }
        private void ResetOther()
        {
            OtherNumber = "";
            OnPropertyChanged(nameof(OtherNumber));
        }

        public void ResetCards()
        {
            Payment.IsValid = true;
            if (RequestType == RequestTypes.Standard)
                Payment.Value = AmountDue;
            ResetCheck();
            ResetOther();
            CreditCardDetails?.ResetCreditCard();
        }
    }
}
