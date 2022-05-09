using System;
using System.Collections.Generic;
using System.Text;

namespace Visum.Services.Mobile.Entities
{
    public class PaymentRequest
    {
        public enum RequestTypes
        {
            Standard = 0,
            StandardDepost = 1,
            SecurityDeposit = 2,
            PreAuthDeposit = 3
        }
        public RequestTypes RequestType { get; set; }
        public string Source { get; set; } // FC = Front Counter, WO = Work Order
        public int SourceID { get; set; } // OrderNo, WONo
        public int CustomerNo { get; set; } 
        public int PaymentTNo { get; set; }
        public decimal PaymentAmt { get; set; } // PaymentAmt - CashBackAmt = Amount Applied
        public decimal CashBackAmt { get; set; } // Not all Types supported
        public decimal TaxAmt { get; set; } 
        public int OnFileNo { get; set; }

        public PaymentRequestOther Other { get; set; }
        public PaymentRequestCheck Check { get; set; }
        public PaymentRequesteCheck eCheck { get; set; }
        public PaymentRequestCard Card { get; set; }
    }

    // "CP", "MS", "IR", "OT", "DS"
    public class PaymentRequestOther
    {
        public string Number { get; set; }
    }

    // "CK" (NOT ACH)
    public class PaymentRequestCheck
    {
        public string Number { get; set; }
        public string DLNumber { get; set; }
        public string DLState { get; set; }
    }

    // "CK" (ACH)
    public class PaymentRequesteCheck
    {
        public enum AccountTypes
        {
            Checking = 0,
            Savings = 1
        }

        public enum CheckTypes
        {
            Personal = 0,
            Business = 1
        }

        public string AccountHolder { get; set; }
        public string AccountNumber { get; set; }
        public string RoutingNumber { get; set; }
        public AccountTypes AccountType { get; set; }
        public CheckTypes CheckType { get; set; }
        public string DLNumber { get; set; }
        public string DLState { get; set; }
        public string SSNLast4 { get; set; }
        public bool  StoreInfo { get; set; }
    }

    // "CC"
    public class PaymentRequestCard
    {
        public string CardHolder { get; set; }
        public string LastFour { get; set; }
        public string Expiration { get; set; }
        public string AuthCode { get; set; }
        public string Street { get; set; }
        public string Zipcode { get; set; }
        public bool StoreInfo { get; set; }
        public string ManualToken { get; set; }
        public bool OnLine { get; set; }
    }

}
