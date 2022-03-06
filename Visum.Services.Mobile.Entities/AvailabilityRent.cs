using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Visum.Services.Mobile.Entities
{
    [DataContract()]
    public class AvailabilityRent
    {
        [DataMember]
        public string AvailDscr { get; set; }

        [DataMember]
        public int AvailItem { get; set; }

        [DataMember]
        public string AvailEquipID { get; set; }

        [DataMember]
        public int AvailCmp { get; set; }

        [DataMember]
        public int AvailOwnQty { get; set; }

        [DataMember]
        public int AvailTransQty { get; set; }

        [DataMember]
        public int AvailShopQty { get; set; }

        [DataMember]
        public short AvailGroup { get; set; }

        [DataMember]
        public int AvailSubGroup { get; set; }

        [DataMember]
        public int AvailRentedQty { get; set; }

        [DataMember]
        public decimal AvailMinChg { get; set; }

        [DataMember]
        public decimal AvailMinChg2 { get; set; }

        [DataMember]
        public decimal AvailMinChg3 { get; set; }

        [DataMember]
        public decimal AvailHourChg { get; set; }

        [DataMember]
        public decimal AvailDayChg { get; set; }

        [DataMember]
        public decimal AvailMultiDayChg { get; set; }

        [DataMember]
        public decimal AvailWeekChg { get; set; }

        [DataMember]
        public decimal AvailMonthChg { get; set; }

        [DataMember]
        public decimal AvailOverChg { get; set; }

        [DataMember]
        public decimal AvailSatMonChg { get; set; }

        [DataMember]
        public decimal AvailWK2Chg { get; set; }

        [DataMember]
        public decimal AvailOpenChg { get; set; }

        [DataMember]
        public bool AvailLate { get; set; }

        [DataMember]
        public bool AvailOverBooked { get; set; }

        [DataMember]
        public bool AvailOnRent { get; set; }

        public Nullable<DateTime> AvailLastRented { get; set; }
        [DataMember(Name = "AvailLastRented")]
        private string strAvailLastRented { get; set; }

        [DataMember]
        public bool AvailCheck { get; set; }

        [DataMember]
        public int AvailQty { get; set; }

        [DataMember]
        public bool AvailOnPickup { get; set; }

        [DataMember]
        public string AvailWebURL { get; set; }

        [DataMember]
        public string AvailRenTrainID { get; set; }

        [DataMember]
        public string AvailSubGroupDscr { get; set; }

        [DataMember]
        public double AvailMeter { get; set; }

        [DataMember]
        public bool AvailSerialized { get; set; }

        [DataMember]
        public bool AvailKit { get; set; }

        [DataMember]
        public int AvailCmpSort { get; set; }

        [DataMember]
        public string AvailBarcode { get; set; }

        [DataMember]
        public string AvailBin { get; set; }

        [DataMember]
        public string AvailPart { get; set; }

        [DataMember]
        public string AvailTurnType { get; set; }

        [DataMember]
        public Nullable<Guid> AvailImage { get; set; }

        [DataMember]
        public Nullable<Guid> AvailSubGroupImage { get; set; }

        [OnSerializing]
        private void OnSerializing(StreamingContext ctx)
        {
            if (this.AvailLastRented != null)
                this.strAvailLastRented = this.AvailLastRented.Value.ToString("g", CultureInfo.InvariantCulture);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            if (string.IsNullOrEmpty(this.strAvailLastRented) == false)
                this.AvailLastRented = DateTime.ParseExact(this.strAvailLastRented, "g", CultureInfo.InvariantCulture);
        }
    }

}
