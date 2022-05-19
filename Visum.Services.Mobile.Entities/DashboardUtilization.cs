using System;
using System.Collections.Generic;
using System.Text;

namespace Visum.Services.Mobile.Entities
{
   public class DashboardUtilization
    {
        public int AllRentalCnt { get; set; }
        public decimal AllRentalCost { get; set; }
        public int AllOnRentCnt { get; set; }
        public decimal AllOnRentCost { get; set; }
        public decimal AllOnRentCntPct { get; set; }
        public decimal AllOnRentCostPct { get; set; }
        public int SerialRentalCnt { get; set; }
        public decimal SerialRentalCost { get; set; }
        public int SerialOnRentCnt { get; set; }
        public decimal SerialOnRentCost { get; set; }
        public decimal SerialRentalCntPct { get; set; }
        public decimal SerialRentalCostPct { get; set; }
        public int NotOnRentCnt { get; set; }
        public decimal NotOnRentCost { get; set; }
        public decimal NotOnRentCostPct { get; set; }
    }
}
