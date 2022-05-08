using System.Collections.Generic;

namespace Visum.Services.Mobile.Entities
{
    public class StoreSettings
    {
        public string OrderTitle { get; set; }
        public string AvailRates { get; set; }
        public List<int> CashCustomers { get; set; }
    }
}
