using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FocalPoint.Data.DataModel
{
    [Table("tblCompany")]
    public class Store
    {
        [PrimaryKey, Unique, Column("CmpNo")]
        public int CmpNo { get; set; }
        [MaxLength(50), Column("CmpName")]
        public string CmpName { get; set; }
        public string DisplayName { get; set; }
    }
}
