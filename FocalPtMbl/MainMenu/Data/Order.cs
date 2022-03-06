using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPtMbl.MainMenu.Data
{
    public class Order
    {
        public int OrderNo { get; set; }
        public int OrderCmp { get; set; }
        public string OrderType { get; set; }
        public string OrderNumberT { get; set; }
        public string CustomerName { get; set; }
        public int OrderCustNo { get; set; }
        public System.DateTime OrderODte { get; set; }
        public System.DateTime OrderDDte { get; set; }
        public decimal OrderAmount { get; set; }
        public System.Nullable<int> RowRank { get; set; }
        public string OrderPO { get; set; }
        public string OrderCustJobNo { get; set; }
        public string TaxDscr { get; set; }
        public string OrderTaxExempt { get; set; }
        public decimal OrderDisc { get; set; }
        public bool OrderDWFlag { get; set; }
        public string AddUsrLName { get; set; }
        public int OrderAddUsr { get; set; }
        public string OrderEvent { get; set; }
        public string SalesName1 { get; set; }
        public string CustomerContact { get; set; }
        public string CustomerAddr1 { get; set; }
        public string CustomerAddr2 { get; set; }
        public string CustomerAddr3 { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerState { get; set; }
        public string CustomerZip { get; set; }
        public int CustomerCountry { get; set; }
        public string CustomerPhoneType { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerPhoneType2 { get; set; }
        public string CustomerPhone2 { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerType { get; set; }
        public string TermsDscr { get; set; }
        public decimal CustomerLimit { get; set; }
        public decimal CustomerARBal { get; set; }
        public OrderDtl[] OrderDtls { get; set; }
        public decimal TotalRentalAmt { get; set; }
        public decimal TotalMerchAmt { get; set; }
        public decimal TotalRentalSaleAmt { get; set; }
        public decimal TotalRentalReplAmt { get; set; }
        public decimal OrderDelAmt { get; set; }
        public decimal OrderPUAmt { get; set; }
        public decimal TotalCleaningAmt { get; set; }
        public decimal TotalDWAmt { get; set; }
        public decimal TotalAddChgAmt { get; set; }
        public decimal TotalMeterAmt { get; set; }
        public decimal TotalFuelAmt { get; set; }
        public decimal OrderTax { get; set; }
        public decimal OrderPaid { get; set; }
        public decimal OrderDeposit { get; set; }
        public decimal TotalSugDepositAmt { get; set; }
        public decimal TotalSecDepositAmt { get; set; }
        public decimal TotalDueAmt { get; set; }
        public decimal OrderDepositRequested { get; set; }
        public System.Nullable<System.DateTime> OrderDepositDueDte { get; set; }
        public Payment[] Payments { get; set; }
        public string ShipToDscr { get; set; }
        public string ShipToContact { get; set; }
        public string ShipToPhone { get; set; }
        public string ShipToAddr1 { get; set; }
        public string ShipToAddr2 { get; set; }
        public string ShipToAddr3 { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToState { get; set; }
        public string ShipToZip { get; set; }
        public int ShipToCountry { get; set; }
        public System.Nullable<System.DateTime> DelDispatchPreferDte { get; set; }
        public string OrderDelFree { get; set; }
        public System.Nullable<System.DateTime> DelDispatchRangeStartDte { get; set; }
        public System.Nullable<System.DateTime> DelDispatchRangeEndDte { get; set; }
        public string OrderDelNotes { get; set; }
        public System.Nullable<System.DateTime> PUDispatchPreferDte { get; set; }
        public string OrderPUFree { get; set; }
        public System.Nullable<System.DateTime> PUDispatchRangeStartDte { get; set; }
        public System.Nullable<System.DateTime> PUDispatchRangeEndDte { get; set; }
        public string OrderPUNotes { get; set; }
        public string OrderStatus { get; set; }
    }

    public class OrderDtl
    {
        public int OrderNo { get; set; }
        public int OrderDtlLine { get; set; }
        public int OrderDtlNewLine { get; set; }
        public string OrderDtlType { get; set; }
        public string OrderDtlDscr { get; set; }
        public string OrderDtlDscr2 { get; set; }
        public decimal OrderDtlAmt { get; set; }
        public decimal OrderDtlQty { get; set; }

        /// <summary>
        /// V4
        /// </summary>
        public int OrderDtlNo { get; set; }
    }

    public class Payment
    {
        public int PaymentNo { get; set; }
        public string PayID { get; set; }
        public decimal PaymentAmt { get; set; }
        public System.DateTime PaymentPDte { get; set; }
        public bool PaymentSD { get; set; }
        public string PaymentDscr { get; set; }
    }
}
