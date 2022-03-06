using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FocalPoint.Data.DataModel
{
	[Table("tblVendor")]
	public class Vendor
	{
		[PrimaryKey, Unique, Column("No")]
		public int No { get; set; }

		[MaxLength(64), Column("Name")]
		public string Name { get; set; }

		[MaxLength(64), Column("Addr1")]
		public string Addr1 { get; set; }

		[MaxLength(64), Column("Addr2")]
		public string Addr2 { get; set; }

		[MaxLength(64), Column("City")]
		public string City { get; set; }

		[MaxLength(16), Column("Zip")]
		public string Zip { get; set; }

		[MaxLength(2), Column("State")]
		public string State { get; set; }

		[MaxLength(256), Column("URL")]
		public string URL { get; set; }

		[MaxLength(64), Column("Contact")]
		public string Contact { get; set; }

		[MaxLength(128), Column("Email")]
		public string Email { get; set; }

		[Column("Phone")]
		public long? Phone { get; set; }

		[Column("Ext")]
		public string Ext { get; set; }

		[Column("Fax")]
		public long? Fax { get; set; }

		[Column("Account")]
		public string Account { get; set; }

		[Column("Credit")]
		public decimal Credit { get; set; }

		[Column("MinPO")]
		public decimal MinPO { get; set; }

		[Column("Notes")]
		public string Notes { get; set; }

		[Column("LastUpdated")]
		public string LastUpdated { get; set; }

		[Ignore()]
		public bool IsSelected { get; set; }
	}
}
