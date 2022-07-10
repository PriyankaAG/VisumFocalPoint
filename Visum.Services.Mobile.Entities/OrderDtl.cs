using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace Visum.Services.Mobile.Entities
{
    public class OrderDtl
    {
        public int OrderNo { get; set; }
        public short OrderDtlLine { get; set; }
        public short OrderDtlNewLine { get; set; }
        public string OrderDtlType { get; set; }
        public string OrderDtlDscr { get; set; }
        public string OrderDtlDscr2 { get; set; }
        public decimal OrderDtlAmt { get; set; }
        public decimal OrderDtlQty { get; set; }
        public decimal OrderDtlDiscount { get; set; }
        public string OrderDtlStatus { get; set; }
        public int OrderDtlNo { get; set; }
        public decimal OrderDtlMeterOut { get; set; }
        public bool OrderDtlGeneric { get; set; }
        public bool OrderDtlParty { get; set; }
        public bool OrderDtlORide { get; set; }
        public decimal OrderDtlCost { get; set; }
        public bool OrderDtlTaxable { get; set; }
        public string OrderDtlNotes { get; set; }
        public List<string> Serials { get; set; } = new List<string>();

        public decimal OrderDtlTax { get; set; }
        public decimal OrderDtlOrgAmt { get; set; }
        public short OrderDtlPackage { get; set; }
        public string PackageDscr { get; set; }
        public decimal OrderDtlMin { get; set; }
        public short OrderDtlMinHr { get; set; }
        public decimal OrderDtlMin2 { get; set; }
        public short OrderDtlMinHr2 { get; set; }
        public decimal OrderDtlMin3 { get; set; }
        public short OrderDtlMinHr3 { get; set; }
        public decimal OrderDtlHour { get; set; }
        public decimal OrderDtlDay { get; set; }
        public bool OrderDtlDayWithMin { get; set; }
        public decimal OrderDtlMultiDay { get; set; }
        public decimal OrderDtlWeek { get; set; }
        public decimal OrderDtlMonth { get; set; }
        public decimal OrderDtlOver { get; set; }
        public decimal OrderDtlSatMon { get; set; }
        public decimal OrderDtlWK2 { get; set; }
        public decimal OrderDtlOpen { get; set; }
        public short OrderDtlMultiDayDays { get; set; }
        public int OrderDtlDaysInWeek { get; set; }

        public bool OrderDtlComm { get; set; }
        public bool OrderDtlSTax { get; set; }
        public string OrderDtlPrint { get; set; }
        public string OrderDtlDisc { get; set; }

        public bool OrderDtlDW { get; set; }
        public decimal OrderDtlDWAmt { get; set; }
        public decimal OrderDtlDWTax { get; set; }

        public string OrderDtlMeterType { get; set; }
        public string IntervalDscr { get; set; }
        public decimal OrderDtlMeterIn { get; set; }
        public decimal OrderDtlMeterChg { get; set; }
        public decimal OrderDtlMeterTax { get; set; }
        public bool OrderDtlPreBillM { get; set; }
        public decimal OrderDtlMeterQty { get; set; }

        public bool OrderDtlFuel { get; set; }
        public string FuelDscr { get; set; }
        public string OrderDtlFuelType { get; set; }
        public decimal OrderDtlFuelAmt { get; set; }
        public decimal OrderDtlFuelTax { get; set; }
        public bool OrderDtlPreBillF { get; set; }
        public decimal OrderDtlFuelQty { get; set; }
        public bool OrderDtlSold { get; set; }

    }
}
