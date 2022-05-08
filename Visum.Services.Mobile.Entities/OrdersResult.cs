using System.Collections.Generic;

namespace Visum.Services.Mobile.Entities
{
    public class OrdersResult
    {
        public List<Order> Orders { get; set; }
        public int Total { get; set; }
    }
}
