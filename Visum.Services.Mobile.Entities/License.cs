using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Visum.Services.Mobile.Entities
{
    public class License
    {
        public enum Statuses
        {
            Valid = 0,
            Invalid = 1,
            Disabled = 2,
            TrialEnded = 3,
            Expired = 4,
            ActivationLimit = 5
        }

        public enum SerialResults
        {
            Success = 0,
            Failed = 1,
            NotASerial = 2,
            InvalidSerial = 3,
            ConnectionError = 4,
            ActivationLimit = 5
        }

        [DataMember()]
        public string Type { get; set; }

        [DataMember()]
        public Statuses Status { get; set; }

        [DataMember()]
        public bool Trial { get; set; }

        [DataMember()]
        public string CustomerName { get; set; }

        [DataMember()]
        public int CustomerID { get; set; }
  
        public DateTime ExpireDate { get; set; }
        [DataMember(Name = "ExpireDate")]
        private string strExpireDate { get; set; }

        [DataMember()]
        public int TrialDays { get; set; }

        [DataMember()]
        public int TrialDaysLeft { get; set; }

        [DataMember()]
        public string RegCode { get; set; }

        [DataMember()]
        public string LicCode { get; set; }

        [DataMember()]
        public string SerialCode { get; set; }

        [OnSerializing]
        private void OnSerializing(StreamingContext ctx)
        {
            this.strExpireDate = this.ExpireDate.ToString("g", CultureInfo.InvariantCulture);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            this.ExpireDate = DateTime.ParseExact(this.strExpireDate, "g", CultureInfo.InvariantCulture);
        }
    }
}
