using System.Collections.Generic;

namespace Visum.Services.Mobile.Entities
{
    public class CustomerOrderSettings
    {
        public string CustomerTaxNo { get; set; } // OrderTaxExempt
        public List<string> CustomerNotifications { get; set; } = new List<string>();
        public string CustomerMessageHTML { get; set; }
    }

}
