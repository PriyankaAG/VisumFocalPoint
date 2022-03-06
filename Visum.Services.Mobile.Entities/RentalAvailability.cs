using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Visum.Services.Mobile.Entities
{
    [DataContract()]
    public class RentalAvailability
    {
        [DataMember]
        public int AvailItem { get; set; }

        [DataMember]
        public string AvailDscr { get; set; }

        [DataMember]
        public string AvailEquipID { get; set; }

        [DataMember]
        public int AvailCmp { get; set; }

        [DataMember]
        public int AvailSubGroup { get; set; }

        [DataMember]
        public decimal AvailOwnQty { get; set; }

        [DataMember]
        public decimal AvailRentedQty { get; set; }

        [DataMember]
        public decimal AvailShopQty { get; set; }

        [DataMember]
        public decimal AvailQty { get; set; }

        public DateTime StartDate { get; set; }
        [DataMember(Name = "StartDate")]
        private string strStartDate { get; set; }

        public DateTime EndDate { get; set; }
        [DataMember(Name = "EndDate")]
        private string strEndDate { get; set; }

        [DataMember]
        public string ResultText { get; set; }

        [OnSerializing]
        private void OnSerializing(StreamingContext ctx)
        {
            this.strStartDate = this.StartDate.ToString("g", CultureInfo.InvariantCulture);
            this.strEndDate = this.EndDate.ToString("g", CultureInfo.InvariantCulture);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            this.StartDate = DateTime.ParseExact(this.strStartDate, "g", CultureInfo.InvariantCulture);
            this.EndDate = DateTime.ParseExact(this.strEndDate, "g", CultureInfo.InvariantCulture);
        }

    }
}
