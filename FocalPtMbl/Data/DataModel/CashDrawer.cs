using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPoint.Data.DataModel
{
	public class CashDrawer
	{
		public decimal StartAmount { get; set; }

		public decimal EndAmount { get; set; }

		public decimal Deposit { get; set; }

		public short DepAcct { get; set; }

		public string DepAcctName { get; set; }

		public decimal CashIn { get; set; }

		public decimal CashOut { get; set; }

		public decimal CheckIn { get; set; }

		public decimal CheckOut { get; set; }

		public decimal CreditCardIn { get; set; }

		public decimal CreditCardOut { get; set; }

		public decimal OtherIn { get; set; }

		public decimal OtherOut { get; set; }

		public decimal RefundIn { get; set; }

		public decimal RefundOut { get; set; }
	}
}
