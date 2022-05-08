using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Visum.Services.Mobile.Entities
{
    [DataContract()]
    public class RentalSettings
    {
        [DataMember]
        public List<DisplayValueString> IntervalTypes { get; set; } = new List<DisplayValueString>();
        [DataMember]
        public List<DisplayValueString> FuelTypes { get; set; } = new List<DisplayValueString>();

    }
}
