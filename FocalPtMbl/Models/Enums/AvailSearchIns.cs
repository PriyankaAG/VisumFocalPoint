using System.ComponentModel;

namespace FocalPoint.Models.Enums
{
    public enum AvailSearchIns
    {
        [Description("Description")]
        Dscr = 1,

        [Description("Part Number")]
        PartNo = 2,

        [Description("Equipment ID")] 
        EquipID = 3,

        [Description("Barcode")]
        Barcode = 4,

        [Description("Serial Number")]
        Serial = 5,

        [Description("Rental Rate")]
        SubGroup = 6,

        [Description("Group")]
        Group = 7,

        [Description("Item Number")]
        Item = 8,

        [Description("UPC Number")]
        UPC = 9,

        [Description("Manufacturer")]
        Mfg = 10,

        [Description("SKU Number")]
        SKU = 11,

        [Description("Retail Price")]
        RetailAmt = 12,

        [Description("Extended")]
        Extended = 13,

        [Description("Exact Equipment ID")]
        EquipIDExact = 14,

        [Description("Rental Rate ID")]
        SubGroupItem = 15,

        [Description("Part History")]
        HistoryID = 16,

        [Description("Top 20")]
        TopXStore = 17,

        [Description("Tiles")]
        Tiles = 18
    }
}
