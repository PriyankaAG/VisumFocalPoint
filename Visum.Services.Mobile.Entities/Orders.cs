using System.Collections.Generic;

namespace Visum.Services.Mobile.Entities
{
    public class Orders
    {
        public int TotalCnt { get; set; }
        public List<Order> List { get; set; } = new List<Order>();
    }
}
