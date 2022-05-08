using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Visum.Services.Mobile.Entities
{
    [DataContract()]
    public class Rental
    {
        [DataMember]
        public int RentalItem { get; set; }

        [DataMember]
        public int RentalCmp { get; set; }

        [DataMember]
        public int RentalBelongsTo { get; set; }

        [DataMember]
        public string RentalDscr { get; set; }

        [DataMember]
        public string RentalEquipID { get; set; }

        [DataMember]
        public int RentalSubGroup { get; set; }

        [DataMember]
        public decimal YTDPct { get; set; }

        [DataMember]
        public decimal LTDPct { get; set; }

        [DataMember]
        public decimal RentalYTDAmt { get; set; }

        [DataMember]
        public decimal RentalLTDAmt { get; set; }

        [DataMember]
        public int RentalYTDUnit { get; set; }

        [DataMember]
        public int RentalLTDUnit { get; set; }

        public DateTime? RentalFirstRented { get; set; }
        [DataMember(Name = "RentalFirstRented")]
        private string strRentalFirstRented { get; set; }

        public DateTime? RentalLastRented { get; set; }
        [DataMember(Name = "RentalLastRented")]
        private string strRentalLastRented { get; set; }

        [DataMember]
        public decimal RentalRepairYTD { get; set; }

        [DataMember]
        public decimal RentalRepairLTD { get; set; }

        [DataMember]
        public decimal RentalCost { get; set; }

        [DataMember]
        public decimal RentalRetail { get; set; }

        [DataMember]
        public string RentalMake { get; set; }

        [DataMember]
        public string RentalModel { get; set; }

        [DataMember]
        public int? RentalYr { get; set; }

        [DataMember]
        public string MfgName { get; set; }

        [DataMember]
        public string RentalNote { get; set; }

        [DataMember]
        public double RentalMeter { get; set; }

        [OnSerializing]
        private void OnSerializing(StreamingContext ctx)
        {
            if (this.RentalFirstRented != null)
                this.strRentalFirstRented = this.RentalFirstRented.Value.ToString("g", CultureInfo.InvariantCulture);
            if (this.RentalLastRented != null)
                this.strRentalLastRented = this.RentalLastRented.Value.ToString("g", CultureInfo.InvariantCulture);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            if (string.IsNullOrEmpty(this.strRentalFirstRented) == false)
                this.RentalFirstRented = DateTime.ParseExact(this.strRentalFirstRented, "g", CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(this.strRentalLastRented) == false)
                this.RentalLastRented = DateTime.ParseExact(this.strRentalLastRented, "g", CultureInfo.InvariantCulture);
        }
    }
}
