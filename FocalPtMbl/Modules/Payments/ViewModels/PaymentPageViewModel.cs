using System.Threading.Tasks;
using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using static Visum.Services.Mobile.Entities.PaymentRequest;
using Visum.Services.Mobile.Entities;
using System.Collections.Generic;
using System;
using System.Linq;

namespace FocalPoint.Modules.Payments.ViewModels
{
    public class PaymentPageViewModel : CommonViewModel
    {
        private PaymentSettings _settings;
        public IPaymentEntityComponent PaymentEntityComponent { get; set; }
        public RequestTypes RequestType { get; set; }
        public List<string> DepositTypes { get; set; }
        public string PaymentMethod { get; set; }

        private List<string> paymentTypes;
        public List<string> PaymentTypes
        {
            get { return paymentTypes; }
            set
            {
                paymentTypes = value;
                OnPropertyChanged(nameof(PaymentTypes));
            }
        }
        public string DepositType { get; set; }
        public string PaymnetType { get; set; }
        public bool IsOtherType { get; set; }
        public bool IsCheck { get; set; }
        public bool IsCheckACH { get; set; }
        public bool IsCreditCard { get; set; }

        #region const
        public PaymentPageViewModel()
        {
            PaymentEntityComponent = new PaymentEntityComponent();
            SetDepositTypes();
            //SetPaymenyTypes();

            GetSettings().ContinueWith((a)=> { _settings = a.Result; });
        }
        #endregion

        private async Task<PaymentSettings> GetSettings()
        {
            return await PaymentEntityComponent.GetPaymentSettings();
        }

        public async Task SetPaymenyTypes(int selectedIndex)
        {
            List<PaymentType> lstPaymentTypes = await GetPaymentTypes(selectedIndex);
            if (lstPaymentTypes != null && lstPaymentTypes.Any())
                PaymentTypes = lstPaymentTypes.Select(x => x.PaymentKind).ToList();
            else
                PaymentTypes = new List<string>();
        }

        private void SetDepositTypes()
        {
            DepositTypes = Enum.GetNames(typeof(RequestTypes)).ToList();
        }

        internal void SetCardView(string selectedItem)
        {
            IsCheck = IsCreditCard = IsOtherType = IsCheckACH = false;
            switch (selectedItem)
            {
                case "CK":
                    if (_settings.ACHEnabled)
                        IsCheck = true;
                    else
                        IsCheckACH = true;
                    break;
                case "CC":
                    IsCreditCard = true;
                    break;
                case "CP":
                case "MS":
                case "IR":
                case "OT":
                case "DS":
                    IsOtherType = true;
                    break;
            }
            OnPropertyChanged(nameof(IsCheck));
            OnPropertyChanged(nameof(IsCreditCard));
            OnPropertyChanged(nameof(IsOtherType));

            /*var otherTypes = new string[] { "CP", "MS", "IR", "OT", "DS" };
            IsOtherType = otherTypes.Contains(selectedItem);
            IsCheck = selectedItem == "CK";
            IsCreditCard = selectedItem == "CC";*/
        }

        public async Task<List<PaymentType>> GetPaymentTypes(int type)
        {
            return await PaymentEntityComponent.GetPaymentTypes(type);
        }

    }
}
