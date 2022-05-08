using System.Collections.Generic;

namespace Visum.Services.Mobile.Entities
{
    public class WorkOrders
    {
        public int TotalCnt { get; set; }
        public List<WorkOrder> List { get; set; } = new List<WorkOrder>();
    }
}
