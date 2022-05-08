using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FocalPoint.Data.DataModel
{
    [Table("tblCustomer")]
    public class Customer
    {
        [PrimaryKey, Unique, Column("CustomerNo")]
        public int CustomerNo { get; set; }
        [Indexed, Column("CustomerStore")]
        public int CustomerStore { get; set; }
        [MaxLength(35), Column("CustomerName")]
        public string CustomerName { get; set; }
        [MaxLength(40), Column("CustomerAddr1")]
        public string CustomerAddr1 { get; set; }
        [MaxLength(40), Column("CustomerAddr2")]
        public string CustomerAddr2 { get; set; }
        [MaxLength(35), Column("CustomerCity")]
        public string CustomerCity { get; set; }
        [MaxLength(16), Column("CustomerZip")]
        public string CustomerZip { get; set; }
        [MaxLength(2), Column("CustomerState")]
        public string CustomerState { get; set; }
        [MaxLength(40), Column("CustomerBillAddr1")]
        public string CustomerBillAddr1 { get; set; }
        [MaxLength(40), Column("CustomerBillAddr2")]
        public string CustomerBillAddr2 { get; set; }
        [MaxLength(35), Column("CustomerBillCity")]
        public string CustomerBillCity { get; set; }
        [MaxLength(16), Column("CustomerBillZip")]
        public string CustomerBillZip { get; set; }
        [MaxLength(2), Column("CustomerBillState")]
        public string CustomerBillState { get; set; }
        [Column("CustomerPhone")]
        public Int64? CustomerPhone { get; set; }
        [MaxLength(2), Column("CustomerPhoneType")]
        public string CustomerPhoneType { get; set; }
        [Column("CustomerPhone2")]
        public Int64? CustomerPhone2 { get; set; }
        [MaxLength(2), Column("CustomerPhoneType2")]
        public string CustomerPhoneType2 { get; set; }
        [Column("CustomerPhone3")]
        public Int64? CustomerPhone3 { get; set; }
        [MaxLength(2), Column("CustomerPhoneType3")]
        public string CustomerPhoneType3 { get; set; }
        [MaxLength(100), Column("CustomerEmail")]
        public string CustomerEmail { get; set; }
        [MaxLength(100), Column("CustomerEmail2")]
        public string CustomerEmail2 { get; set; }
        [MaxLength(100), Column("CustomerEmail3")]
        public string CustomerEmail3 { get; set; }
        [MaxLength(1), Column("CustomerType")]
        public string CustomerType { get; set; }
        [Column("CustomerNotes")]
        public string CustomerNotes { get; set; }
        [Column("CustomerLastUpdated")]
        public string CustomerLastUpdated { get; set; }

        [Column("CustomerStatus")]
        public string CustomerStatus { get; set; }
        [Column("CustomerStatusDisplay")]
        public string CustomerStatusDisplay { get; set; }
        [Column("CustomerTypeDisplay")]
        public string CustomerTypeDisplay { get; set; }

        [Ignore()]
        public bool IsSelected { get; set; }
    }
}
