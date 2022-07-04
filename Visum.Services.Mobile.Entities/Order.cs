using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Visum.Services.Mobile.Entities
{
    [DataContract()]
    public class Order
    {
        [DataMember]
        public int OrderNo { get; set; }

        [DataMember]
        public int OrderCmp { get; set; }

        [DataMember]
        public string OrderType { get; set; }

        [DataMember]
        public string OrderNumberT { get; set; }

        [DataMember]
        public int OrderCustNo { get; set; }

        public DateTime OrderODte { get; set; }
        [DataMember(Name = "OrderODte")]
        private string strOrderODte { get; set; }

        public DateTime OrderDDte { get; set; }
        [DataMember(Name = "OrderDDte")]
        private string strOrderDDte { get; set; }

        [DataMember]
        public decimal OrderAmount { get; set; }

        [DataMember]
        public string OrderPO { get; set; }

        [DataMember]
        public string OrderCustJobNo { get; set; }

        [DataMember]
        public string TaxCodeDscr { get; set; }

        [DataMember]
        public string OrderTaxExempt { get; set; }

        [DataMember]
        public decimal OrderDisc { get; set; }

        [DataMember]
        public bool OrderDiscountable { get; set; }

        [DataMember]
        public bool OrderDWFlag { get; set; }

        [DataMember]
        public string AddUsrLName { get; set; }

        [DataMember]
        public int OrderAddUsr { get; set; }

        [DataMember]
        public string OrderEvent { get; set; }

        [DataMember]
        public string SalesName1 { get; set; }

        [DataMember]
        public string TermsDscr { get; set; }

        [DataMember]
        public decimal OrderDelAmt { get; set; }

        [DataMember]
        public decimal OrderPUAmt { get; set; }

        [DataMember]
        public decimal OrderTax { get; set; }

        [DataMember]
        public decimal OrderPaid { get; set; }

        [DataMember]
        public decimal OrderDeposit { get; set; }

        [DataMember]
        public decimal OrderDepositRequested { get; set; }

        public DateTime? OrderDepositDueDte { get; set; }
        [DataMember(Name = "OrderDepositDueDte")]
        private string strOrderDepositDueDte { get; set; }

        [DataMember]
        public string OrderDelFree { get; set; }

        [DataMember]
        public string OrderDelNotes { get; set; }

        [DataMember]
        public string OrderPUFree { get; set; }

        [DataMember]
        public string OrderPUNotes { get; set; }

        [DataMember]
        public string OrderStatus { get; set; }

        [DataMember]
        public string StatusDscr { get; set; }

        [DataMember]
        public string OrderLength { get; set; }

        [DataMember]
        public string LengthDscr { get; set; }

        [DataMember]
        public int OrderEDays { get; set; }

        [DataMember]
        public short OrderEventRate { get; set; }

        [DataMember]
        public int OrderTaxCode { get; set; }

        [DataMember]
        public bool OrderDfltCust { get; set; }

        [DataMember]
        public string OrderNotes { get; set; }

        [DataMember]
        public string OrderIntNotes { get; set; }

        [DataMember]
        public Customer Customer { get; set; }

        [DataMember]
        public ShipTo ShipTo { get; set; }

        [DataMember]
        public Dispatch DelDispatch { get; set; }

        [DataMember]
        public Dispatch PUDispatch { get; set; }

        [DataMember]
        public OrderTotals Totals { get; set; } = new OrderTotals();

        [DataMember]
        public List<OrderDtl> OrderDtls { get; set; } = new List<OrderDtl>();

        [DataMember]
        public List<Payment> Payments { get; set; } = new List<Payment>();

        [DataMember]
        public int? RowRank { get; set; }

        [OnSerializing]
        private void OnSerializing(StreamingContext ctx)
        {
            if (this.OrderDepositDueDte != null)
                this.strOrderDepositDueDte = this.OrderDepositDueDte.Value.ToString("g", CultureInfo.InvariantCulture);
            this.strOrderODte = this.OrderODte.ToString("g", CultureInfo.InvariantCulture);
            this.strOrderDDte = this.OrderDDte.ToString("g", CultureInfo.InvariantCulture);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            if (string.IsNullOrEmpty(this.strOrderDepositDueDte) == false)
                this.OrderDepositDueDte = DateTime.ParseExact(this.strOrderDepositDueDte, "g", CultureInfo.InvariantCulture);
            this.OrderODte = DateTime.ParseExact(this.strOrderODte, "g", CultureInfo.InvariantCulture);
            this.OrderDDte = DateTime.ParseExact(this.strOrderDDte, "g", CultureInfo.InvariantCulture);
        }
    }

}
