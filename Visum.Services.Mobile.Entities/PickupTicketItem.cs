using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Visum.Services.Mobile.Entities
{
    [DataContract()]
    public class PickupTicketItem
    {
        [DataMember]
        public int PuTNo { get; set; }

        [DataMember]
        public int PuDtlTNo { get; set; }

        [DataMember]
        public string OrderDtlDscr { get; set; }

        [DataMember]
        public string EquipID { get; set; }

        [DataMember]
        public decimal PuDtlQty { get; set; }

        [DataMember]
        public decimal OrderDtlQty { get; set; }

        [DataMember]
        public string OrderDtlMeterType { get; set; }

        [DataMember]
        public string OrderDtlFuelType { get; set; }

        [DataMember]
        public decimal PuDtlCntQty { get; set; }

        [DataMember]
        public decimal PuDtlOutQty { get; set; }

        [DataMember]
        public decimal PuDtlSoldQty { get; set; }

        [DataMember]
        public decimal PuDtlStolenQty { get; set; }

        [DataMember]
        public decimal PuDtlLostQty { get; set; }

        [DataMember]
        public decimal PuDtlDmgdQty { get; set; }

        [DataMember]
        public double PuDtlMeterIn { get; set; }

        [DataMember]
        public double PuDtlTank { get; set; }

        [DataMember]
        public bool PuDtlCounted { get; set; }

        [DataMember]
        public int[] RowVersion { get; set; }

        public DateTime? UTCCountDte { get; set; }
        [DataMember(Name = "UTCCountDte")]
        private string strUTCCountDte { get; set; }

        [DataMember]
        public string LastCntEmpID { get; set; }

        public DateTime? LastCntDte { get; set; }
        [DataMember(Name = "LastCntDte")]
        private string strLastCntDte { get; set; }

        [DataMember]
        public decimal LastCntQty { get; set; }

        [DataMember]
        public decimal LastCntOutQty { get; set; }

        [DataMember]
        public decimal LastCntSoldQty { get; set; }

        [DataMember]
        public decimal LastCntStolenQty { get; set; }

        [DataMember]
        public decimal LastCntLostQty { get; set; }

        [DataMember]
        public decimal LastCntDmgdQty { get; set; }
        [DataMember]
        public decimal CurrentTotalCnt { get; set; }
        [DataMember]
        public string ImageName { get; set; }

        [IgnoreDataMember]
        public decimal TotalCounted { get; set; }

        [IgnoreDataMember()]
        public bool Checked { get; set; }

        [OnSerializing]
        private void OnSerializing(StreamingContext ctx)
        {
            if (this.UTCCountDte != null)
                this.strUTCCountDte = this.UTCCountDte.Value.ToString("g", CultureInfo.InvariantCulture);
            if (this.LastCntDte != null)
                this.strLastCntDte = this.LastCntDte.Value.ToString("g", CultureInfo.InvariantCulture);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            if (string.IsNullOrEmpty(this.strUTCCountDte) == false)
                this.UTCCountDte = DateTime.ParseExact(this.strUTCCountDte, "g", CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(this.strLastCntDte) == false)
                this.LastCntDte = DateTime.ParseExact(this.strLastCntDte, "g", CultureInfo.InvariantCulture);
        }
        public object Clone()
        {
            return MemberwiseClone();
        }

    }
}
