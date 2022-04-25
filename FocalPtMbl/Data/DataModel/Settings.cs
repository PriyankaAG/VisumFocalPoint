using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FocalPoint.Data.DataModel
{
    [Table("tblSettings")]
    public class Settings
    {
        [PrimaryKey, Unique, Column("ID")]
        public int ID { get; set; }

        [MaxLength(35), Column("Host")]
        public string Host { get; set; }

        [MaxLength(35), Column("UserName")]
        public string UserName { get; set; }

        [MaxLength(35), Column("UserPassword")]
        public string UserPassword { get; set; }

        [MaxLength(35), Column("UserToken")]
        public string UserToken { get; set; }

        [MaxLength(35), Column("User")]
        public string User { get; set; }

        [Column("HomeStore")]
        public int HomeStore { get; set; }

        [Column(nameof(Terminal))]
        public int Terminal { get; set; }

        [MaxLength(35), Column("AppVersion")]
        public string AppVersion { get; set; }

        [MaxLength(35), Column("SerialCode")]
        public string SerialCode { get; set; }
        [MaxLength(35), Column("LicenseCode")]
        public string LicenseCode { get; set; }
        [Column("LocalIP")]
        public string LocalIP { get; set; }
        [Column("ApiVersion")]
        public int ApiVersion { get; set; }
        [Column("DbVersion")]
        public int DbVersion { get; set; }
        [Column("OrderTitle")]
        public string OrderTitle { get; set; }
        [Column("ApiSvcVersion")]
        public int ApiSvcVersion { get; set; }
        [Column("ApiUri")]
        public string ApiUri { get; set; }
    }
}
