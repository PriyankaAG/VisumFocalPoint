namespace Visum.Services.Mobile.Entities
{
    public class AvailabilityKitDetail
    {
        public int AvailKitID { get; set; }
        public int AvailKitDtlID { get; set; }
        public bool AvailKitMaster { get; set; }
        public string AvailDscr { get; set; }
        public int AvailItem { get; set; }
        public string AvailEquipID { get; set; }
        public string AvailType { get; set; }
        public short AvailGroup { get; set; }
        public int AvailSubGroup { get; set; }
        public string AvailMfg { get; set; }
        public string AvailPart { get; set; }
        public bool AvailRequired { get; set; }
        public bool AvailIncCount { get; set; }
        public decimal AvailSuggestQty { get; set; }
        public string AvailPrint { get; set; }
        public string AvailCharge { get; set; }
        public int AvailCmp { get; set; }
        public decimal AvailQty { get; set; }
        public decimal AvailStoreQty { get; set; }
        public decimal AvailOwnQty { get; set; }
        public decimal AvailStoreOwnQty { get; set; }
        public decimal AvailRentedQty { get; set; }
        public decimal AvailStoreRentedQty { get; set; }
        public decimal AvailShopQty { get; set; }
        public decimal AvailStoreShopQty { get; set; }
        public decimal AvailTransQty { get; set; }
        public decimal AvailStoreTransQty { get; set; }
        public bool AvailOnPickup { get; set; }
        public bool AvailStoreOnPickup { get; set; }
        public string AvailWebURL { get; set; }
        public short AvailKitSort { get; set; }
    }

}
