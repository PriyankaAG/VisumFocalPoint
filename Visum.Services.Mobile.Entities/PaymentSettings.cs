using System;
using System.Collections.Generic;
using System.Text;

namespace Visum.Services.Mobile.Entities
{
    public class PaymentSettings
    {
        public bool POSEnabled { get; set; }
        public bool ACHEnabled { get; set; }
        public bool CardOnFile { get; set; }
        public bool eCheckOnFile { get; set; }
        public decimal CashDrawerStartBal { get; set; }
    }
}
