using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace Visum.Services.Mobile.Entities
{
    [DataContract()]
    public class OrderSettings
    {
        [DataMember]
        public OrderDefaults Defaults { get; set; } = new OrderDefaults();

        [DataMember]
        public List<DisplayValueInteger> TaxCodes { get; set; } = new List<DisplayValueInteger>();

        [DataMember]
        public List<DisplayValueString> Lengths { get; set; } = new List<DisplayValueString>();

        [DataMember]
        public bool OverbookingAllowed { get; set; }

        [DataMember]
        public List<RateTypes> AvailiblityRates { get; set; }
    }

    [DataContract()]
    public class OrderDefaults
    {
        [DataMember]
        public int OrderCustNo { get; set; }

        [DataMember]
        public string OrderType { get; set; }

        [DataMember]
        public string OrderLength { get; set; }
        
        public DateTime OrderODte { get; set; }
        [DataMember(Name = "OrderODte")]
        private string strOrderODte { get; set; }

        public DateTime OrderDDte { get; set; }
        [DataMember(Name = "OrderDDte")]
        private string strOrderDDte { get; set; }

        [DataMember]
        public int OrderEDays { get; set; }

        [DataMember]
        public short OrderEventRate { get; set; }

        [DataMember]
        public int OrderTaxCode { get; set; }

        [OnSerializing]
        private void OnSerializing(StreamingContext ctx)
        {
            this.strOrderODte = this.OrderODte.ToString("g", CultureInfo.InvariantCulture);
            this.strOrderDDte = this.OrderDDte.ToString("g", CultureInfo.InvariantCulture);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            this.OrderODte = DateTime.ParseExact(this.strOrderODte, "g", CultureInfo.InvariantCulture);
            this.OrderDDte = DateTime.ParseExact(this.strOrderDDte, "g", CultureInfo.InvariantCulture);
        }

    }
}
