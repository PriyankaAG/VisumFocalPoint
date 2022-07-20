using System;
using System.Collections.Generic;
using System.Text;

namespace Visum.Services.Mobile.Entities
{
    public class DashboardHome
    {
        public int OrdersDayCnt { get; set; }
        public decimal OrdersDayAmt { get; set; }
        public int OrdersWeekCnt { get; set; }
        public decimal OrdersWeekAmt { get; set; }
        public int OrdersMonthCnt { get; set; }
        public decimal OrdersMonthAmt { get; set; }
        public int RsrvsDayCnt { get; set; }
        public decimal RsrvsDayItemCnt { get; set; }
        public int RsrvsWeekCnt { get; set; }
        public decimal RsrvsWeekItemCnt { get; set; }
        public int RsrvsMonthCnt { get; set; }
        public decimal RsrvsMonthItemCnt { get; set; }
        public decimal DueBackDayItemCnt { get; set; }
        public decimal DueBackTmrwItemCnt { get; set; }
        public decimal DueBackWeekItemCnt { get; set; }
        public decimal DlvrDayItemCnt { get; set; }
        public decimal DlvrTmrwItemCnt { get; set; }
        public decimal DlvrWeekItemCnt { get; set; }
        public decimal PckpDayItemCnt { get; set; }
        public decimal PckpTmrwItemCnt { get; set; }
        public decimal PckpWeekItemCnt { get; set; }
        public int WrkOrdIntOpen { get; set; }
        public int WrkOrdIntPrmsd { get; set; }
        public int WrkOrdIntCmplt { get; set; }
        public int WrkOrdIntMsd { get; set; }
        public int WrkOrdCustOpen { get; set; }
        public int WrkOrdCustPrmsd { get; set; }
        public int WrkOrdCustCmplt { get; set; }
        public int WrkOrdCustMsd { get; set; }
    }
}
