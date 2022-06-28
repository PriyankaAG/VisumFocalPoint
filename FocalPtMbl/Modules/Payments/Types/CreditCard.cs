using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FocalPoint.Components.Interface;
using FocalPoint.Validations;
using FocalPoint.Validations.Rules;
using FocalPtMbl.MainMenu.ViewModels;
using MvvmHelpers.Commands;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.Payments.Types
{
    public class CreditCard : ThemeBaseViewModel
    {
        const string CARD_ON_FILE_TEXT = "Using Card On File";
        Order order;
        public IPaymentEntityComponent PaymentEntityComponent;
        public PaymentInfo StoredCardInfo;
        public CreditCard(Order order, IPaymentEntityComponent paymentEntityComponent, PaymentSettings settings)
        {
            this.order = order;
            Settings = settings;
            PaymentEntityComponent = paymentEntityComponent;
            SetProcessOnline();
            CreditCardPaymentDetails = new CreditCardPaymentDetails
            {
                Header = "Card on File",
                CreditCardDetailList = new ObservableCollection<PaymentInfo>()
            };
            Task.Run(async () =>
            {
                if (order != null)
                {
                    await GetCreditCardList();
                }
            });
            CardHolderName = new ValidatableObject<string>();
            CardLast4Digits = new ValidatableObject<string>();
            AuthorizationCode = new ValidatableObject<string>();
            AvsStreetAddress = new ValidatableObject<string>();
            AvsZipCode = new ValidatableObject<string>();
            CardOnFileText = "";

            AddValidations();
            CardDetailSelectCommand = new Command<PaymentInfo>((PaymentInfo info) => UpdateDetails(info));
        }

        private void SetProcessOnline() => ProcessOnline = Settings.POSEnabled ? true : false; //defaults to true od Credit card POS
        #region Properties
        public ValidatableObject<string> CardHolderName { get; set; }
        public ValidatableObject<string> CardLast4Digits { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.MinValue;
        public ValidatableObject<string> AuthorizationCode { get; set; }
        public ValidatableObject<string> AvsStreetAddress { get; set; }
        public ValidatableObject<string> AvsZipCode { get; set; }
        public string ManualToken { get; set; }
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
        private string _cardOnFileText;
        public string CardOnFileText 
        {
            get { return _cardOnFileText; }
            set 
            {
                _cardOnFileText = value;
                OnPropertyChanged(nameof(CardOnFileText));
            }
        }
        public PaymentSettings Settings { get; set; }
        #endregion

        #region Commands
        public ICommand CardDetailSelectCommand { get; }
        public ICommand ValidateCardHolderNameCommand => new Command(() => ValidateField(CardHolderName));
        public ICommand ValidateCardLast4DigitsCommand => new Command(() => ValidateField(CardLast4Digits));
        public ICommand ValidateAuthorizationCodeCommand => new Command(() => ValidateField(AuthorizationCode));
        public ICommand ValidateAvsStreetAddressCommand => new Command(() => ValidateField(AvsStreetAddress));
        public ICommand ValidateAvsZipCodeCommand => new Command(() => ValidateField(AvsZipCode));
        #endregion

        private bool ValidateField(ValidatableObject<string> validatableField)
        {
            return validatableField.Validate();
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

        private void AddValidations()
        {
            CardHolderName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Validation failed. Please fill required data." });
            CardLast4Digits.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Validation failed. Please fill required data." });
            CardLast4Digits.Validations.Add(new IsCardLastFourRule<string> { ValidationMessage = "Please enter the last 4 of the Credit Card Number" });
            AuthorizationCode.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Validation failed. Please fill required data." });
            AvsStreetAddress.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Validation failed. Please fill required data." });
            AvsZipCode.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Validation failed. Please fill required data." });
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

        private void UpdateDetails(PaymentInfo info)
        {
            StoredCardInfo = info;
            CardHolderName.Value = info.InfoHolder;
            CardLast4Digits.Value = info.InfoText;
            if (DateTime.TryParseExact(info.InfoExpireDte, "MMddyy", CultureInfo.InvariantCulture,
                           DateTimeStyles.None, out DateTime date))
                ExpirationDate = date;
            SetProcessOnline();
            IsStoredCardSelected = true;
            CardOnFileText = CARD_ON_FILE_TEXT;
            OnPropertyChanged(nameof(CardHolderName));
            OnPropertyChanged(nameof(CardLast4Digits));
            OnPropertyChanged(nameof(ExpirationDate));
            OnPropertyChanged(nameof(ProcessOnline));
            OnPropertyChanged(nameof(IsStoredCardSelected));
        }

        public string ValidateCreditCardDetails()
        {
            if (Settings.POSType == 0 || Settings.POSType > 0 && !ProcessOnline)
            {
                ValidateField(CardHolderName);
                ValidateField(CardLast4Digits);
                ValidateField(AuthorizationCode);
                if (!(CardHolderName.IsValid && CardLast4Digits.IsValid && AuthorizationCode.IsValid))
                    return "Validation failed. Please fill required data.";
                if (!CardLast4Digits.IsValid)
                    return CardLast4Digits.Errors?.First() ?? "Validation failed.";
                if(ExpirationDate < DateTime.MinValue)
                    return "Validation failed. Please fill required data.";
            }
            if (ExpirationDate < DateTime.MinValue && DateTime.Compare(ExpirationDate, DateTime.Now) < 0)
                return "Credit Card Expired!";
            return "";

            //ValidateField(CardHolderName);
            //ValidateField(CardLast4Digits);
            //if (Settings.POSEnabled)
            //{
            //    ValidateField(AvsStreetAddress);
            //    ValidateField(AvsZipCode);
            //    if (!(CardHolderName.IsValid && AvsStreetAddress.IsValid && AvsZipCode.IsValid))
            //        return "Validation failed. Please correct data.";
            //}
            //else
            //{
            //    ValidateField(AuthorizationCode);
            //    if (!(CardHolderName.IsValid && AuthorizationCode.IsValid))
            //        return "Validation failed. Please fill required data.";
            //}
            //if (!CardLast4Digits.IsValid)
            //    return CardLast4Digits.Errors?.First() ?? "Validation failed.";
            //if (DateTime.Compare(ExpirationDate, DateTime.Now) < 0)
            //    return "Credit Card Expired!";
            //return "";
        }

        public PaymentRequestCard GetCardDetails()
        {
            return new PaymentRequestCard
            {
                CardHolder = CardHolderName.Value,
                LastFour = CardLast4Digits.Value,
                Expiration = ExpirationDate.ToString(),
                AuthCode = ProcessOnline ? null : AuthorizationCode.Value,
                OnLine = ProcessOnline,
                Street = ProcessOnline ? AvsStreetAddress.Value : null,
                Zipcode = ProcessOnline ? AvsZipCode.Value : null,
                StoreInfo = StoreCardOnFile,
                ManualToken = ManualToken
            };
        }
        public void ResetCreditCard()
        {
            CardHolderName.Value = CardLast4Digits.Value = AuthorizationCode.Value
                = AvsStreetAddress.Value = AvsZipCode.Value = ManualToken = "";
            CardHolderName.IsValid = CardLast4Digits.IsValid = AuthorizationCode.IsValid
                = AvsStreetAddress.IsValid = AvsZipCode.IsValid = true;
            StoreCardOnFile = false;
            //ExpirationDate = DateTime.Now;
            IsStoredCardSelected = false;
            CardOnFileText = "";
            SetProcessOnline();
            OnPropertyChanged(nameof(CardHolderName));
            OnPropertyChanged(nameof(CardLast4Digits));
            OnPropertyChanged(nameof(AuthorizationCode));
            OnPropertyChanged(nameof(AvsStreetAddress));
            OnPropertyChanged(nameof(AvsZipCode));
            OnPropertyChanged(nameof(StoreCardOnFile));
            //OnPropertyChanged(nameof(ExpirationDate));
            OnPropertyChanged(nameof(IsStoredCardSelected));
            OnPropertyChanged(nameof(ProcessOnline));
            OnPropertyChanged(nameof(ManualToken));
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
