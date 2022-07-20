using System;
using System.Collections.Generic;
using System.Text;

namespace Visum.Services.Mobile.Entities
{
    public class KitDetail
    {
        public int KitID { get; set; }
        public int KitDtlID { get; set; }
        public string KitType { get; set; }
        public int KitItem { get; set; }
        public int KitSubGroup { get; set; }
        public byte KitSubGroupType { get; set; }
        public string KitDscr { get; set; }
        public bool KitRequired { get; set; }
        public decimal KitStoreQty { get; set; }
        public decimal KitStoreAvailQty { get; set; }
        public decimal KitOwnQty { get; set; }
        public decimal KitRentedQty { get; set; }
        public decimal KitShopQty { get; set; }
        public decimal KitTransQty { get; set; }
        public decimal KitQty { get; set; }
        public int KitItemCnt { get; set; }
        public bool KitPrint { get; set; }
        public bool KitCharge { get; set; }
        public bool KitIncluded { get; set; }
        public decimal KitIncludedQty { get; set; }
        public int KitIncludedItem { get; set; }
        public int KitIncludedSubGroup { get; set; }
        public bool KitIncludedGeneric { get; set; }
        public bool KitIncludedAdded { get; set; }
        public short KitSort { get; set; }
        public string KitDscrOrg { get; set; }
    }
}
