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
        public int SecArea { get; set; }
        [Column("SecAllowed")]
        public bool SecAllowed { get; set; }
    }
}
