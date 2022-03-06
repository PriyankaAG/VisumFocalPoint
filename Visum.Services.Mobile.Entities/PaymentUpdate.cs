using System;
using System.Collections.Generic;
using System.Text;

namespace Visum.Services.Mobile.Entities
{
    public class PaymentUpdate
    {
        public Payment Payment { get; set; }
        public List<string> Notifications { get; set; } = new List<string>();
    }
}
