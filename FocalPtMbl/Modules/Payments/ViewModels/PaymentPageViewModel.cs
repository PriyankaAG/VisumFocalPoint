using System.Threading.Tasks;
using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using static Visum.Services.Mobile.Entities.PaymentRequest;
using Visum.Services.Mobile.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Windows.Input;
using MvvmHelpers.Commands;
using FocalPoint.Modules.Payments.Views;

namespace FocalPoint.Modules.Payments.ViewModels
{
    public class PaymentPageViewModel : CommonViewModel
    {
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
        public bool IsOtherType { get; set; }
        public bool IsCash { get; set; }
        public bool IsCheck { get; set; }
        public bool IsCreditCard { get; set; }
        public bool IsCreditCardPOS { get; set; }
        public bool IsDebitCard { get; set; }
        public bool ShowDueAndReceived { get; set; }
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

        #region const
        public PaymentPageViewModel()
        {
            PaymentEntityComponent = new PaymentEntityComponent();
            PaymentTypeSelection = new Command<int>((paymentType) => SetPaymentSelectionType(paymentType));
            ProcessOnline = true;
            //settings = await GetSettings();
            GetSettings().ContinueWith((a) => { settings = a.Result; });
        }
        #endregion

        public async void SetPaymentSelectionType(int paymentType)
        {

        }

        private async Task<PaymentSettings> GetSettings()
        {
            return await PaymentEntityComponent.GetPaymentSettings();
        }

        public async Task SetPaymenyTypes(int selectedIndex)
        {
            List<PaymentType> lstPaymentTypes = await PaymentEntityComponent.GetPaymentTypes(selectedIndex);
            if (lstPaymentTypes != null && lstPaymentTypes.Any())
            {
                PaymentTypes = lstPaymentTypes;
                OnPropertyChanged(nameof(PaymentTypes));
            }
        }

        internal async void SetCardView(PaymentType paymentType)
        {
            IsCash = IsCheck = IsCreditCard = IsOtherType = IsCreditCardPOS = IsDebitCard = false;
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
                        //IsCardOnFile = settings.CardOnFile && ProcessOnline;
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
                case "DC":
                    IsDebitCard = true;
                    break;
            }
            OnPropertyChanged(nameof(IsCash));
            OnPropertyChanged(nameof(IsCheck));
            OnPropertyChanged(nameof(IsCreditCard));
            OnPropertyChanged(nameof(IsOtherType));
            OnPropertyChanged(nameof(IsCreditCardPOS));
            OnPropertyChanged(nameof(IsDebitCard));
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
                //1 => "american_express_96.png",
                1 => "Blank_96.png",
                2 => "cash_96.png",
                3 => "check_96.png",
                4 => "Coupon_96.png",
                5 => "credit_cards_96.png",
                6 => "cirrus_96.png",
                7 => "diners_club_96.png",
                8 => "discover_96.png",
                9 => "e_check_96.png",
                10 => "gift_card_96.png",
                11 => "gift_card_96.png",
                12 => "mastercard_96.png",
                13 => "paypal_96.png",
                14 => "purchase_order_96.png",
                //15 => "vis_96.png",
                15 => "Blank_96.png",
                _ => "Blank_96.png",
            };
        }

    }
}
