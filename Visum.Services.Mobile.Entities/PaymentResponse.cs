using System;
using System.Collections.Generic;
using System.Text;

namespace Visum.Services.Mobile.Entities
{
    public class PaymentResponse
    {
        public Payment Payment { get; set; }
        public bool GetSignature { get; set; }
        public List<Payment> Payments { get; set; } = new List<Payment>();
        public List<string> Notifications { get; set; } = new List<string>();

    }
}
