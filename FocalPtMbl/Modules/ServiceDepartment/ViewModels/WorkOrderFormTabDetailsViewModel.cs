using FocalPoint.Data.API;
using FocalPoint.Modules.FrontCounter.ViewModels;
using FocalPoint.Modules.FrontCounter.Views;
using FocalPtMbl.MainMenu.ViewModels;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.ServiceDepartment.ViewModels
{
    public class WorkOrderFormTabDetailsViewModel : CommonViewModel
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
            SetEntityDetails(DocKinds.WorkOrder, workOrder.WONo, "W");
        }
        /// <summary>
        /// Header
        /// </summary>

        public int WONo
        {
            get => order.WONo;
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
            get => this.order.WOODte.ToString();
        }
        public string WOCDte
        {
            get => this.order.WOCDte.ToString();
        }
        public string WOPDte
        {
            get => this.order.WOPDte.ToString();
        }
        public string WOPUDte
        {
            get => this.order.WOPUDte.ToString();
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
            get => this.order.WODelDte.ToString();
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
    }
}