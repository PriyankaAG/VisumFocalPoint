using System;
using System.Collections.Generic;
using System.Text;

namespace Visum.Services.Mobile.Entities
{
    public class PaymentType
    {
        public int PaymentTNo { get; set; }
        public string PaymentKind { get; set; }
        public string PaymentDscr { get; set; }
        public string PaymentID { get; set; }
        public byte PaymentTIcon { get; set; }
        public bool PaymentTACHOnly { get; set; }

    }
}
