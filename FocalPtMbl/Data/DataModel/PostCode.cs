using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FocalPoint.Data.DataModel
{
    [Table("tblPostCode")]
    public class PostCode
    {
        [PrimaryKey, Unique, Column("PostCodeNo")]
        public int PostCodeNo { get; set; }
        [MaxLength(50), Column("PostCodeDscr")]
        public string PostCodeDscr { get; set; }
    }
}
