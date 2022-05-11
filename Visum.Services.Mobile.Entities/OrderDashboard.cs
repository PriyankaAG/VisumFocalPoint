using System;
using System.Collections.Generic;
using System.Text;

namespace Visum.Services.Mobile.Entities
{
    public class OrderDashboard
    {
        public int DayCnt { get; set; }
        public decimal DayAmt { get; set; }
        public int WeekCnt { get; set; }
        public decimal WeekAmt { get; set; }
        public int MonthCnt { get; set; }
        public decimal MonthAmt { get; set; }
        public List<OrderDashboardOverview> Overviews { get; set; } = new List<OrderDashboardOverview>();
        public DashboardUtilization Utilization { get; set; }
    }
}
