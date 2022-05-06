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
        public int OpenCnt { get; set; }
        public int OpenEquipCnt { get; set; }
        public List<Order> OpenOrders { get; set; } = new List<Order>();
        public int RsrvCnt { get; set; }
        public int RsrvEquipCnt { get; set; }
        public List<Order> RsrvOrders { get; set; } = new List<Order>();
        public int ReturnCnt { get; set; }
        public int ReturnEquipCnt { get; set; }
        public List<Order> ReturnOrders { get; set; } = new List<Order>();
    }
}
