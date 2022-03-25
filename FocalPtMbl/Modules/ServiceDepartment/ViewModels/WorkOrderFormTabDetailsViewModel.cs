using FocalPoint.Data.API;
using FocalPoint.Modules.FrontCounter.ViewModels;
using FocalPoint.Modules.FrontCounter.Views;
using FocalPoint.Utils;
using FocalPtMbl.MainMenu.ViewModels;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.ServiceDepartment.ViewModels
{
    public class WorkOrderFormTabDetailsViewModel : ThemeBaseViewModel
    {
        readonly WorkOrder order;
        public WorkOrderFormTabDetailsViewModel(WorkOrder workOrder)
        {
            GeneralComponent = new GeneralComponent();
            foreach (var paym in workOrder.Payments)
                this.Payments.Add(paym);
            foreach (var dtl in workOrder.WorkOrderDtls)
                this.Details.Add(dtl);
            this.order = workOrder;
        }
        /// <summary>
        /// Header
        /// </summary>

        public int WONo
        {
            get => order.WONo;
        }
        public string _signatureImage;
        public string SignatureImage
        {
            get => _signatureImage;
            set => SetProperty(ref _signatureImage, value);
        }
        private string _waiverCapturedImage;
        public string WaiverCapturedImage
        {
            get => _waiverCapturedImage;
            set => SetProperty(ref _waiverCapturedImage, value);
        }
        public string WODscr
        {
            get => this.order.WODscr;
        }
        public string WOTaxNo
        {
            get => this.order.WOTaxNo;
        }
        public string TaxDscr
        {
            get => this.order.TaxDscr;
        }

        public string WOPO
        {
            get => this.order.WOPO;
        }

        public string WOJobNo
        {
            get => this.order.WOJobNo;
        }

        public string WOTechID
        {
            get => this.order.WOTechID;
        }
        public string WORepairDscr
        {
            get => this.order.WORepairDscr;
        }
        public string WOODte
        {
            get => this.order.WOODte.ToFormattedDate();
        }
        public string WOCDte
        {
            get => this.order.WOCDte.ToFormattedDate();
        }
        public string WOPDte
        {
            get => this.order.WOPDte.ToFormattedDate();
        }
        public string WOPUDte
        {
            get => this.order.WOPUDte.ToFormattedDate();
        }

        private SignatureMessageOutputDTO _signatureMessageOutputDTO;
        public SignatureMessageOutputDTO SignatureMessageOutputDTO
        {
            get => _signatureMessageOutputDTO;
            set => SetProperty(ref _signatureMessageOutputDTO, value);
        }
        public decimal WOPUAmt
        {
            get => this.order.WOPUAmt;
        }
        public string WODelDte
        {
            get => this.order.WODelDte.ToFormattedDate();
        }
        public decimal WODelAmt
        {
            get => this.order.WODelAmt;
        }

        public IGeneralComponent GeneralComponent { get; set; }


        /// <summary>
        /// Customer
        /// </summary>
        public Customer Customer
        {
            get => this.order.Customer;
        }
        public string CustomerName
        {
            get => this.order.Customer.CustomerName;
        }
        public string CustomerContact
        {
            get => this.order.Customer.CustomerContact;
        }
        public string AddressText1
        {
            get => this.order.Customer.CustomerAddr1;
        }

        public string AddressText2
        {
            get => this.order.Customer.CityStateZip + ";";
        }
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public string Phone1Header
        {
            get => GetPhoneHeadder(this.order.Customer.CustomerPhoneType);
        }
        private string GetPhoneHeadder(string type)
        {
            if (type == "F")
                return "Fax:";
            else if (type == "C")
                return "Cell:";
            else if (type == "H")
                return "Home:";
            else if (type == "W")
                return "Work:";
            else
                return "";
        }
        public string CustomerPhone1
        {
            get => FormatNumber(this.order.Customer.CustomerPhone);
        }
        private string FormatNumber(string phoneNumber)
        {
            string newNumber = "";
            if (phoneNumber != null)
            {
                if (phoneNumber.Length > 0)
                    newNumber = Regex.Replace(phoneNumber, @"(\d{3})(\d{3})(\d{4})", "($1)$2-$3");
            }
            return newNumber;
        }

        public string Phone2Header
        {
            get => GetPhoneHeadder(this.order.Customer.CustomerPhoneType2);
        }
        public string CustomerPhone2
        {
            get => FormatNumber(this.order.Customer.CustomerPhone2);
        }
        public string CustomerEmail
        {
            get => this.order.Customer.CustomerEmail;
        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////
        /// </summary>
        public string CustomerTypeText
        {
            get => GetCustomerType(this.order.Customer.CustomerType);
        }
        private string GetCustomerType(string type)
        {
            if (type.ToUpper() == "F")
                return "Credit";
            else if (type.ToUpper() == "C")
                return "Cash";
            else if (type.ToUpper() == "H")
                return "Home";
            else
                return "";
        }
        public string TermsDscr
        {
            get => this.order.TermsDscr;
        }
        public decimal CustomerLimit
        {
            get => this.order.Customer.CustomerLimit;
        }
        public decimal CustomerARBal
        {
            get => this.order.Customer.CustomerARBal;
        }
        /// <summary>
        /// ItemInfoPage
        /// </summary>
        /// //////////////////////////////////////////////////////////////////////
        public string ItemType
        {
            get => this.order.WODscr;
        }
        public string WOItemDscr
        {
            get => this.order.WOItemDscr;
        }
        public string WOEquipID
        {
            get => this.order.WOEquipID;
        }

        public string WOSerial
        {
            get => this.order.WOSerial;
        }

        public string WOMake
        {
            get => this.order.WOMake;
        }

        public string WOModel
        {
            get => this.order.WOModel;
        }
        public double WOMeter
        {
            get => this.order.WOMeter;
        }
        public int? WOYear
        {
            get => this.order.WOYear;
        }
        public string WOColor
        {
            get => this.order.WOColor;
        }
        /// <summary>
        /// NotesInPage
        /// </summary>

        public string WONotesIn
        {
            get => this.order.WONotesIn;
        }

        /// <summary>
        /// Payments
        /// </summary>
        private ObservableCollection<Payment> payments = new ObservableCollection<Payment>();
        public ObservableCollection<Payment> Payments
        {
            get => this.payments;
            set
            {
                if (payments.Count < 0)
                    this.payments = value;
                else
                {
                    this.payments.Clear();
                    this.payments = value;
                }
                OnPropertyChanged(nameof(Payments));
            }

        }

        /// Details
        /// 
        /// 
        private ObservableCollection<WorkOrderDtl> details = new ObservableCollection<WorkOrderDtl>();
        public ObservableCollection<WorkOrderDtl> Details
        {
            get => this.details;
            set
            {
                if (details.Count < 0)
                    this.details = value;
                else
                {
                    this.details.Clear();
                    this.details = value;
                }
                OnPropertyChanged(nameof(Details));
            }

        }

        //public string Line1
        //{
        //    get
        //    {
        //        return this.Detail.WODtlDscr.Trim();
        //    }
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        //public string Line2
        //{
        //    get
        //    {
        //        return string.Format("{0}, {1:C}", this.Detail.WODtlMfg.Trim(), this.Detail.WODtlPart);
        //    }
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        //public string Line3
        //{
        //    get
        //    {
        //        var status = string.Empty;

        //        if (this.Detail.WODtlStatus == "I")
        //            status = " - Internal Charge";
        //        else if (this.Detail.WODtlStatus == "C")
        //            status = " - Customer Charge";

        //        return string.Format("{0:N2} {1:C}{2}", this.Detail.WODtlQty, this.Detail.WODtlPrice, status);
        //    }
        //}
        /// <summary>
        /// NotesInOut
        /// </summary>

        public string WONotesOut
        {
            get => this.order.WONotesOut;
        }

        /// <summary>
        /// Totals
        /// </summary>
        public decimal WOTAmt
        {
            get => this.order.WOTAmt;
        }
        public decimal WOCAmt
        {
            get => this.order.WOCAmt;
        }
        public decimal WOWAmt
        {
            get => this.order.WOWAmt;
        }

        public decimal WOTax
        {
            get => this.order.WOTax;
        }
        public decimal WOPaid
        {
            get => this.order.WOPaid;
        }
        public decimal TotalPartsCustAmt
        {
            get => this.order.Totals.TotalPartsCustAmt;
        }
        public decimal TotalWarLaborCustAmt
        {
            get => this.order.Totals.TotalWarLaborCustAmt;
        }
        public decimal TotalTaxCustAmt
        {
            get => this.order.Totals.TotalTaxCustAmt;
        }
        public decimal TotalPartsIntAmt
        {
            get => this.order.Totals.TotalPartsIntAmt;
        }
        public decimal TotalLaborIntAmt
        {
            get => this.order.Totals.TotalLaborIntAmt;
        }
        public decimal TotalWarPartsCustAmt
        {
            get => this.order.Totals.TotalWarPartsCustAmt;
        }
        public decimal TotalWarPartsIntAmt
        {
            get => this.order.Totals.TotalWarPartsIntAmt;
        }
        public decimal TotalWarLaborIntAmt
        {
            get => this.order.Totals.TotalWarLaborIntAmt;
        }
        public decimal TotalTaxIntAmt
        {
            get => this.order.Totals.TotalTaxIntAmt;
        }
        public decimal TotalLaborCustAmt
        {
            get => this.order.Totals.TotalLaborCustAmt;
        }

        internal bool EmailExists()
        {
            return false;
        }

        public async Task SignatureCommand(INavigation navigation)
        {
            SignatureMessageInputDTO singnatureMessageInputDTO = new SignatureMessageInputDTO
            {
                DocKind = (int)DocKinds.WorkOrder,
                RecordID = WONo,
                Stat = "W"//OrderEdit, used when editing an existing Order
            };
            SignatureMessageOutputDTO = await GeneralComponent.GetSignatureMessageDTO(singnatureMessageInputDTO);
            if (SignatureMessageOutputDTO != null)
            {
                if (!string.IsNullOrWhiteSpace(SignatureMessageOutputDTO.Waiver))
                {
                    OpenSignatureWaiverPage(navigation);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(SignatureMessageOutputDTO.Terms))
                    {
                        OpenSignatureTermsView(navigation);
                    }
                    else
                    {
                        OpenSignaturePage(navigation, false);
                    }
                }
            }
            else
            {
                OpenSignaturePage(navigation, false);
            }
        }

        public void OpenSignatureTermsView(INavigation navigation)
        {
            var orderSignatureTermsViewModel = new OrderSignatureTermsViewModel(false, WONo, "Terms & Conditions", SignatureMessageOutputDTO.Terms);
            var orderSignatureTermsView = new SignatureTermsView();
            orderSignatureTermsView.BindingContext = orderSignatureTermsViewModel;
            navigation.PushAsync(orderSignatureTermsView);
        }

        public void OpenSignatureWaiverPage(INavigation navigation)
        {
            var orderSignatureTermsViewModel = new OrderSignatureTermsViewModel(true, WONo, SignatureMessageOutputDTO.WaiverDscr, SignatureMessageOutputDTO.Waiver);
            var orderSignatureTermsView = new SignatureTermsView();
            orderSignatureTermsView.BindingContext = orderSignatureTermsViewModel;
            navigation.PushAsync(orderSignatureTermsView);
        }

        public void OpenSignaturePage(INavigation navigation, bool isWaiver)
        {
            OrderSignatureViewModel orderSignatureViewModel = new OrderSignatureViewModel(order, isWaiver, "Sign above for Terms & Conditions");
            var orderSignatureView = new OrderSignatureView();
            orderSignatureView.BindingContext = orderSignatureViewModel;
            navigation.PushAsync(orderSignatureView);
        }

        public async Task<bool> SaveSignature()
        {
            SignatureInputDTO signatureInputDTO = new SignatureInputDTO();
            signatureInputDTO.DocKind = (int)DocKinds.WorkOrder;
            signatureInputDTO.RecordID = WONo;
            signatureInputDTO.Stat = "W";//OrderEdit, used when editing an existing Order
            signatureInputDTO.Format = 4;//Base64 String of Image
            signatureInputDTO.Signature = SignatureImage;
            signatureInputDTO.Waiver = WaiverCapturedImage;
            return await GeneralComponent.SaveSignature(signatureInputDTO);
        }

        public void IsNeedToRedirectTermsOrSignature(INavigation navigation)
        {
            if (!string.IsNullOrWhiteSpace(SignatureMessageOutputDTO?.Terms))
            {
                OpenSignatureTermsView(navigation);
            }
            else
            {
                OpenSignaturePage(navigation, false);
            }
        }
    }
}