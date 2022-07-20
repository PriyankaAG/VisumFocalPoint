using System;
using System.Collections.Generic;
using System.Text;

namespace Visum.Services.Mobile.Entities
{
    public class PaymentSettings
    {
        public short POSType { get; set; }
        public string POSPublicKey { get; set; }
        public string POSManualUrl { get; set; }
        public bool POSEnabled { get; set; }
        public bool ACHEnabled { get; set; }
        public bool CardOnFile { get; set; }
        public bool eCheckOnFile { get; set; }
        public bool CardInfoReq { get; set; }
        public decimal CashDrawerStartBal { get; set; }
    }
}
