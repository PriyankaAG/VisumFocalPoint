using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Visum.Services.Mobile.Entities
{
    [DataContract()]
    public class Dispatches
    {
        [DataMember]
        public int DispatchID { get; set; }

        [DataMember]
        public Int16 DispatchType { get; set; }

        public DateTime DispatchPreferDte { get; set; }
        [DataMember(Name = "DispatchPreferDte")]
        private string strDispatchPreferDte { get; set; }

        public DateTime? DispatchStartDte { get; set; }
        [DataMember(Name = "DispatchStartDte")]
        private string strDispatchStartDte { get; set; }

        public DateTime? DispatchEndDte { get; set; }
        [DataMember(Name = "DispatchEndDte")]
        private string strDispatchEndDte { get; set; }

        [DataMember]
        public string DispatchSubject { get; set; }

        [DataMember]
        public int DispatchTruckID { get; set; }

        public DateTime DispatchRangeStartDte { get; set; }
        [DataMember(Name = "DispatchRangeStartDte")]
        private string strDispatchRangeStartDte { get; set; }

        public DateTime DispatchRangeEndDte { get; set; }
        [DataMember(Name = "DispatchRangeEndDte")]
        private string strDispatchRangeEndDte { get; set; }

        [DataMember]
        public string DispatchAddr1 { get; set; }

        [DataMember]
        public string DispatchAddr2 { get; set; }

        [DataMember]
        public string DispatchCity { get; set; }

        [DataMember]
        public string DispatchState { get; set; }

        [DataMember]
        public string DispatchZip { get; set; }

        [DataMember]
        public string DispatchDscr { get; set; }

        [DataMember]
        public int DispatchCmp { get; set; }

        [DataMember]
        public string OriginTimeNotes { get; set; }

        [DataMember]
        public string OriginNotes { get; set; }

        [DataMember]
        public string DispatchOrigin { get; set; }

        [DataMember]
        public string DispatchDuration { get; set; }

        [DataMember]
        public string OriginNumber { get; set; }

        [DataMember]
        public string OriginContact { get; set; }

        [DataMember]
        public string OriginPhone { get; set; }

        [DataMember]
        public string ShipToContact { get; set; }

        [DataMember]
        public string ShipToPhone { get; set; }

        [OnSerializing]
        private void OnSerializing(StreamingContext ctx)
        {
            if (this.DispatchStartDte != null)
                this.strDispatchStartDte = this.DispatchStartDte.Value.ToString("g", CultureInfo.InvariantCulture);
            if (this.DispatchEndDte != null)
                this.strDispatchEndDte = this.DispatchEndDte.Value.ToString("g", CultureInfo.InvariantCulture);
            this.strDispatchPreferDte = this.DispatchPreferDte.ToString("g", CultureInfo.InvariantCulture);
            this.strDispatchRangeStartDte = this.DispatchRangeStartDte.ToString("g", CultureInfo.InvariantCulture);
            this.strDispatchRangeEndDte = this.DispatchRangeEndDte.ToString("g", CultureInfo.InvariantCulture);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            if (string.IsNullOrEmpty(this.strDispatchStartDte) == false)
                this.DispatchStartDte = DateTime.ParseExact(this.strDispatchStartDte, "g", CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(this.strDispatchEndDte) == false)
                this.DispatchEndDte = DateTime.ParseExact(this.strDispatchEndDte, "g", CultureInfo.InvariantCulture);
            this.DispatchPreferDte = DateTime.ParseExact(this.strDispatchPreferDte, "g", CultureInfo.InvariantCulture);
            this.DispatchRangeStartDte = DateTime.ParseExact(this.strDispatchRangeStartDte, "g", CultureInfo.InvariantCulture);
            this.DispatchRangeEndDte = DateTime.ParseExact(this.strDispatchRangeEndDte, "g", CultureInfo.InvariantCulture);
        }
    }
}
