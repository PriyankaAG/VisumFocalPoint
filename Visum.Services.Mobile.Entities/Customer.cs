using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Visum.Services.Mobile.Entities
{
    [DataContract()]
    public class Customer
    {
        [DataMember]
        public int CustomerNo { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string CustomerFName { get; set; }

        [DataMember]
        public string CustomerLName { get; set; }

        [DataMember]
        public string CustomerMI { get; set; }

        [DataMember]
        public string CustomerAddr1 { get; set; }

        [DataMember]
        public string CustomerAddr2 { get; set; }

        [DataMember]
        public string CustomerAddr3 { get; set; }

        [DataMember]
        public string CustomerCity { get; set; }

        [DataMember]
        public string CustomerZip { get; set; }

        [DataMember]
        public string CustomerState { get; set; }

        [DataMember]
        public int CustomerCountry { get; set; }

        [DataMember]
        public string CityStateZip { get; set; }

        [DataMember]
        public string CustomerBillAddr1 { get; set; }

        [DataMember]
        public string CustomerBillAddr2 { get; set; }

        [DataMember]
        public string CustomerBillAddr3 { get; set; }

        [DataMember]
        public string CustomerBillCity { get; set; }

        [DataMember]
        public string CustomerBillZip { get; set; }

        [DataMember]
        public string CustomerBillState { get; set; }

        [DataMember]
        public string BillCityStateZip { get; set; }

        [DataMember]
        public string CustomerPhone { get; set; }

        [DataMember]
        public string CustomerPhoneType { get; set; }

        [DataMember]
        public string CustomerPhone2 { get; set; }

        [DataMember]
        public string CustomerPhoneType2 { get; set; }

        [DataMember]
        public string CustomerPhone3 { get; set; }

        [DataMember]
        public string CustomerPhoneType3 { get; set; }

        [DataMember]
        public string CustomerEmail { get; set; }

        [DataMember]
        public string CustomerEMail2 { get; set; }

        [DataMember]
        public string CustomerEMail3 { get; set; }

        [DataMember]
        public string CustomerType { get; set; }

        [DataMember]
        public string CustomerType_Display { get; set; }

        [DataMember]
        public string CustomerNotes { get; set; }

        [DataMember]
        public string CustomerNotes_HTML { get; set; }

        [DataMember]
        public int CustomerStore { get; set; }

        [DataMember]
        public string CustomerStatus { get; set; }

        [DataMember]
        public string CustomerStatus_Display { get; set; }

        [DataMember]
        public string CustomerContact { get; set; }

        [DataMember]
        public decimal CustomerLimit { get; set; }

        [DataMember]
        public decimal CustomerARBal { get; set; }

        [DataMember]
        public short CustomerTerms { get; set; }

        [DataMember]
        public bool CustomerDW { get; set; }

        [DataMember]
        public string CustomerSMSNumber { get; set; }

        [DataMember]
        public string CustomerTaxNo { get; set; }

        [DataMember]
        public short CustomerCType { get; set; }

        [DataMember]
        public string CustomerDrLic { get; set; }

        public DateTime? CustomerDLEDte { get; set; }
        [DataMember(Name = "CustomerDLEDte")]
        private string strCustomerDLEDte { get; set; }

        [DataMember]
        public string CustomerDrLicSt { get; set; }

        public DateTime? CustomerBDte { get; set; }
        [DataMember(Name = "CustomerBDte")]
        private string strCustomerBDte { get; set; }

        [DataMember]
        public string CustomerSS { get; set; }

        [DataMember]
        public int CustomerFoundNo { get; set; }

        [DataMember]
        public bool CustomerPEMail { get; set; }

        [DataMember]
        public byte CustomerOEMail { get; set; }

        [DataMember]
        public byte CustomerCEMail { get; set; }

        [DataMember]
        public byte CustomerROHEMail { get; set; }

        [DataMember]
        public byte CustomerWOCEMail { get; set; }

        [DataMember]
        public byte CustomerWOFEMail { get; set; }

        [DataMember]
        public byte CustomerReminder { get; set; }

        [DataMember]
        public byte CustomerReminderEvent { get; set; }

        [OnSerializing]
        private void OnSerializing(StreamingContext ctx)
        {
            if (this.CustomerDLEDte != null)
                this.strCustomerDLEDte = this.CustomerDLEDte.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (this.CustomerBDte != null)
                this.strCustomerBDte = this.CustomerBDte.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext ctx)
        {
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            if (string.IsNullOrEmpty(this.strCustomerDLEDte) == false)
                this.CustomerDLEDte = DateTime.ParseExact(this.strCustomerDLEDte, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(this.strCustomerBDte) == false)
                this.CustomerBDte = DateTime.ParseExact(this.strCustomerBDte, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }
}