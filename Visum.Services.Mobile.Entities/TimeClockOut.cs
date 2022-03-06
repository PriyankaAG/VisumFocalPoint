using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Visum.Services.Mobile.Entities
{
    [DataContract()]
    public class TimeClockOut
    {
        [DataMember]

        public string UserID { get; set; }
        [DataMember]
        public int LastClockID { get; set; }

        [DataMember]
        public bool Exception { get; set; }

        public DateTime UTCTime { get; set; }
        [DataMember(Name = "UTCTime")]
        private string strUTCTime { get; set; }

        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public double Longitude { get; set; }

        [OnSerializing]
        private void OnSerializing(StreamingContext ctx)
        {
            this.strUTCTime = this.UTCTime.ToString("g", CultureInfo.InvariantCulture);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            this.UTCTime = DateTime.ParseExact(this.strUTCTime, "g", CultureInfo.InvariantCulture);
        }
    }
}
