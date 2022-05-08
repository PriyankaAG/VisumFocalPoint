using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace Visum.Services.Mobile.Entities
{
    [DataContract()]
    public class PickupTicket
    {
        [DataMember]
        public int PuTNo { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string OrderNumberT { get; set; }

        public DateTime? PuPDte { get; set; }
        [DataMember(Name = "PuPDte")]
        private string strPuPDte { get; set; }

        public DateTime? PuCDte { get; set; }
        [DataMember(Name = "PuCDte")]
        private string strPuCDte { get; set; }

        public DateTime PuEDte { get; set; }
        [DataMember(Name = "PuEDte")]
        private string strPuEDte { get; set; }

        [DataMember]
        public string PuEEmpid { get; set; }

        [DataMember]
        public string PuContact { get; set; }

        [DataMember]
        public string PuNote { get; set; }

        [DataMember]
        public bool PuMobile { get; set; }

        [DataMember]
        public string Address1 { get; set; }

        [DataMember]
        public string Address2 { get; set; }

        [DataMember]
        public string CityStateZip { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string PhoneType { get; set; }

        [DataMember]
        public string Phone2 { get; set; }

        [DataMember]
        public string PhoneType2 { get; set; }

        [DataMember]
        public List<PickupTicketItem> Details { get; set; }

        [OnSerializing]
        private void OnSerializing(StreamingContext ctx)
        {
            if (this.PuPDte != null)
                this.strPuPDte = this.PuPDte.Value.ToString("g", CultureInfo.InvariantCulture);
            if (this.PuCDte != null)
                this.strPuCDte = this.PuCDte.Value.ToString("g", CultureInfo.InvariantCulture);
            this.strPuEDte = this.PuEDte.ToString("g", CultureInfo.InvariantCulture);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            if (string.IsNullOrEmpty(this.strPuPDte) == false)
                this.PuPDte = DateTime.ParseExact(this.strPuPDte, "g", CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(this.strPuCDte) == false)
                this.PuCDte = DateTime.ParseExact(this.strPuCDte, "g", CultureInfo.InvariantCulture);
            this.PuEDte = DateTime.ParseExact(this.strPuEDte, "g", CultureInfo.InvariantCulture);
        }
    }
}
