using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FocalPoint.Data.DataModel
{
    [Table("tblSecurity")]
    public class Security
    {
        [PrimaryKey, Unique, Column("SecArea")]
        public int Area { get; set; }
        [Column("SecAllowed")]
        public bool Allowed { get; set; }
    }
}
