using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FocalPoint.Data.DataModel
{
    [Table("tblCacheHistory")]
    public class CacheHistory
    {
        [PrimaryKey, Unique, MaxLength(50), Column("CacheTable")]
        public string CacheTable { get; set; }
        [Column("CacheDte")]
        public DateTime? CacheDte { get; set; }
    }
}
