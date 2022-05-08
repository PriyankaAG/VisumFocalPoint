using System.Collections.Generic;

namespace Visum.Services.Mobile.Entities
{
    public class AvailabilityKit
    {
        public int AvailKitNo { get; set; }
        public string AvailDscr { get; set; }
        public int AvailItem { get; set; }
        public string AvailEquipID { get; set; }
        public string AvailType { get; set; }
        public short AvailGroup { get; set; }
        public int AvailSubGroup { get; set; }
        public bool AvailPricing { get; set; }
        public decimal AvailMinChg { get; set; }
        public int AvailMinHrs { get; set; }
        public decimal AvailMinChg2 { get; set; }
        public int AvailMinHrs2 { get; set; }
        public decimal AvailMinChg3 { get; set; }
        public int AvailMinHrs3 { get; set; }
        public decimal AvailHourChg { get; set; }
        public decimal AvailDayChg { get; set; }
        public decimal AvailMultiDayChg { get; set; }
        public decimal AvailWeekChg { get; set; }
        public decimal AvailMonthChg { get; set; }
        public decimal AvailOverChg { get; set; }
        public decimal AvailOpenChg { get; set; }
        public decimal AvailSatMonChg { get; set; }
        public decimal AvailWK2Chg { get; set; }
        public int AvailCmpSort { get; set; }
        public int AvailCmp { get; set; }
        public string AvailWebURL { get; set; }
        public decimal AvailSuggestQty { get; set; }
        public decimal AvailPotenQty { get; set; }
        public decimal AvailQty { get; set; }
        public byte AvailSerialType { get; set; }
        public List<AvailabilityKitDetail> AvailDetails { get; set; } = new List<AvailabilityKitDetail>();
    }
}
