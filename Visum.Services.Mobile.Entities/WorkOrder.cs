using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace Visum.Services.Mobile.Entities
{
    [DataContract()]
    public class WorkOrder
    {
        [DataMember]
        public int WONo { get; set; }

        [DataMember]
        public int WOCmp { get; set; }

        [DataMember]
        public int WOCusNo { get; set; }

        [DataMember]
        public string WODscr { get; set; }

        [DataMember]
        public string WOTaxNo { get; set; }

        [DataMember]
        public string TaxDscr { get; set; }

        [DataMember]
        public string WOPO { get; set; }

        [DataMember]
        public string WOJobNo { get; set; }

        [DataMember]
        public string WOTechID { get; set; }

        [DataMember]
        public string WORepairDscr { get; set; }

        public DateTime? WOODte { get; set; }
        [DataMember(Name = "WOODte")]
        private string strWOODte { get; set; }

        public DateTime? WOPDte { get; set; }
        [DataMember(Name = "WOPDte")]
        private string strWOPDte { get; set; }

        public DateTime? WOCDte { get; set; }
        [DataMember(Name = "WOCDte")]
        private string strWOCDte { get; set; }

        public DateTime? WOPUDte { get; set; }
        [DataMember(Name = "WOPUDte")]
        private string strWOPUDte { get; set; }

        public DateTime? WODelDte { get; set; }
        [DataMember(Name = "WODelDte")]
        private string strWODelDte { get; set; }

        [DataMember]
        public decimal WOPUAmt { get; set; }

        [DataMember]
        public decimal WODelAmt { get; set; }

        [DataMember]
        public string WOItemType { get; set; }

        [DataMember]
        public string WOItemDscr { get; set; }

        [DataMember]
        public string WOEquipID { get; set; }

        [DataMember]
        public string WOSerial { get; set; }

        [DataMember]
        public string WOMake { get; set; }

        [DataMember]
        public string WOModel { get; set; }

        [DataMember]
        public double WOMeter { get; set; }

        [DataMember]
        public int? WOYear { get; set; }

        [DataMember]
        public string WOColor { get; set; }

        [DataMember]
        public string TermsDscr { get; set; }

        [DataMember]
        public decimal WOTAmt { get; set; }

        [DataMember]
        public decimal WOCAmt { get; set; }

        [DataMember]
        public decimal WOTax { get; set; }

        [DataMember]
        public decimal WOPaid { get; set; }

        [DataMember]
        public string WONotesIn { get; set; }

        [DataMember]
        public string WONotesOut { get; set; }

        [DataMember]
        public decimal WOWAmt { get; set; }

        [DataMember]
        public string WOType { get; set; }

        [DataMember]
        public Customer Customer { get; set; }

        [DataMember]
        public List<WorkOrderDtl> WorkOrderDtls { get; set; } = new List<WorkOrderDtl>();

        [DataMember]
        public List<Payment> Payments { get; set; } = new List<Payment>();

        [DataMember]
        public WorkOrderTotals Totals { get; set; } = new WorkOrderTotals();

        [DataMember]
        public int? RowRank { get; set; }

        [OnSerializing]
        private void OnSerializing(StreamingContext ctx)
        {
            if (this.WOPDte != null)
                this.strWOPDte = this.WOPDte.Value.ToString("g", CultureInfo.InvariantCulture);
            if (this.WOCDte != null)
                this.strWOCDte = this.WOCDte.Value.ToString("g", CultureInfo.InvariantCulture);
            if (this.WOPUDte != null)
                this.strWOPUDte = this.WOPUDte.Value.ToString("g", CultureInfo.InvariantCulture);
            if (this.WODelDte != null)
                this.strWODelDte = this.WODelDte.Value.ToString("g", CultureInfo.InvariantCulture);
            if (this.WOODte != null)
                this.strWOODte = this.WOODte.Value.ToString("g", CultureInfo.InvariantCulture);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            if (string.IsNullOrEmpty(this.strWOPDte) == false)
                this.WOPDte = DateTime.ParseExact(this.strWOPDte, "g", CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(this.strWOCDte) == false)
                this.WOCDte = DateTime.ParseExact(this.strWOCDte, "g", CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(this.strWOPUDte) == false)
                this.WOPUDte = DateTime.ParseExact(this.strWOPUDte, "g", CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(this.strWODelDte) == false)
                this.WODelDte = DateTime.ParseExact(this.strWODelDte, "g", CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(this.strWOODte) == false)
                this.WOODte = DateTime.ParseExact(this.strWOODte, "g", CultureInfo.InvariantCulture);
        }
    }
}
