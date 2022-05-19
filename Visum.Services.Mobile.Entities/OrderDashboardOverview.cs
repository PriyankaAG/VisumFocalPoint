using System;
using System.Collections.Generic;
using System.Text;

namespace Visum.Services.Mobile.Entities
{
    public class OrderDashboardOverview
    {
        public int DscrID { get; set; }
        public string Dscr { get; set; }
        public int OrderCnt { get; set; }
        public decimal RentalAmt { get; set; }
        public decimal MerchAmt { get; set; }
        public decimal LaborAmt { get; set; }
        public decimal GrossAmt { get; set; }
        public decimal AvgTranAmt { get; set; }
    }
}
