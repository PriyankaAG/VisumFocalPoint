using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Visum.Services.Mobile.Entities
{
    [DataContract()]
    public class Payment
    {
        [DataMember]
        public int PaymentNo { get; set; }

        [DataMember]
        public string PayID { get; set; }

        [DataMember]
        public decimal PaymentAmt { get; set; }

        public DateTime PaymentPDte { get; set; }
        [DataMember(Name = "PaymentPDte")]
        private string strPaymentPDte { get; set; }

        [DataMember]
        public bool PaymentSD { get; set; }

        [DataMember]
        public bool PaymentDeposit { get; set; }

        [DataMember]
        public string PaymentDscr { get; set; }

        [DataMember]
        public bool PaymentVoid { get; set; }

        [DataMember]
        public short PaymentSource { get; set; }

        [OnSerializing]
        private void OnSerializing(StreamingContext ctx)
        {
            this.strPaymentPDte = this.PaymentPDte.ToString("g", CultureInfo.InvariantCulture);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            this.PaymentPDte = DateTime.ParseExact(this.strPaymentPDte, "g", CultureInfo.InvariantCulture);
        }

    }
}
