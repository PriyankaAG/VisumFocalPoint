using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Visum.Services.Mobile.Entities
{
    public class OrderMeters
    {
        [DataMember]
        public List<OrderDtl> OrderDtls { get; set; } = new List<OrderDtl>();

        [DataMember]
        public List<string> Notifications { get; set; } = new List<string >();

    }
}
