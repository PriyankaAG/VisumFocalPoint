using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Visum.Services.Mobile.Entities
{
    [DataContract()]
    public class Dispatch
    {
        public DateTime DispatchPreferDte { get; set; }
        [DataMember(Name = "DispatchPreferDte")]
        private string strDispatchPreferDte { get; set; }

        public DateTime DispatchRangeStartDte { get; set; }
        [DataMember(Name = "DispatchRangeStartDte")]
        private string strDispatchRangeStartDte { get; set; }

        public DateTime DispatchRangeEndDte { get; set; }
        [DataMember(Name = "DispatchRangeEndDte")]
        private string strDispatchRangeEndDte { get; set; }

        [OnSerializing]
        private void OnSerializing(StreamingContext ctx)
        {
            this.strDispatchPreferDte = this.DispatchPreferDte.ToString("g", CultureInfo.InvariantCulture);
            this.strDispatchRangeStartDte = this.DispatchRangeStartDte.ToString("g", CultureInfo.InvariantCulture);
            this.strDispatchRangeEndDte = this.DispatchRangeEndDte.ToString("g", CultureInfo.InvariantCulture);
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext ctx)
        {
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            this.DispatchPreferDte = DateTime.ParseExact(this.strDispatchPreferDte, "g", CultureInfo.InvariantCulture);
            this.DispatchRangeStartDte = DateTime.ParseExact(this.strDispatchRangeStartDte, "g", CultureInfo.InvariantCulture);
            this.DispatchRangeEndDte = DateTime.ParseExact(this.strDispatchRangeEndDte, "g", CultureInfo.InvariantCulture);
        }

    }
}
