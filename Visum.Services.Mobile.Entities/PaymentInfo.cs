using System;
using System.Collections.Generic;
using System.Text;

namespace Visum.Services.Mobile.Entities
{
    public class PaymentInfo
    {
        public int InfoID { get; set; } // ID
        public string InfoText { get; set; } // Last 4
        public string InfoHolder { get; set; } // Account Holder
        public string InfoPaymentID { get; set; } // Payment ID to use
        public string PaymentDscr { get; set; } 
        public string InfoExpireDte { get; set; } // CC Expire Date
        public short InfoType { get; set; } // CC Card Enum
        public string InfoDscr { get; set; } // Defined Description
    }
}
