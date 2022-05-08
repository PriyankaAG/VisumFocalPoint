using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPoint.Data.DataModel
{
	public class RentalValuation
	{
		/// <summary>
		/// 
		/// </summary>
		public decimal TotalAmount { get; set; }


		/// <summary>
		/// 
		/// </summary>
		public int TotalCount { get; set; }


		/// <summary>
		/// 
		/// </summary>
		public decimal RentAmount { get; set; }


		/// <summary>
		/// 
		/// </summary>
		public int RentCount { get; set; }


		/// <summary>
		/// 
		/// </summary>
		public decimal OffRentAmount
		{
			get
			{
				return TotalAmount - RentAmount;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public int OffRentCount
		{
			get
			{
				return TotalCount - RentCount;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public decimal SerialTotalAmount { get; set; }


		/// <summary>
		/// 
		/// </summary>
		public int SerialTotalCount { get; set; }


		/// <summary>
		/// 
		/// </summary>
		public decimal SerialRentAmount { get; set; }


		/// <summary>
		/// 
		/// </summary>
		public int SerialRentCount { get; set; }


		/// <summary>
		/// 
		/// </summary>
		public decimal SerialOffRentAmount
		{
			get
			{
				return SerialTotalAmount - SerialRentAmount;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public int SerialOffRentCount
		{
			get
			{
				return SerialTotalCount - SerialRentCount;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public decimal UtilizationPercentAmount
		{
			get
			{
				if (TotalAmount <= 0)
					return 0;

				return RentAmount / TotalAmount * 100;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public decimal UtilizationPercentCount
		{
			get
			{
				if (TotalCount <= 0)
					return 0;

				return RentCount / TotalCount * 100;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public decimal UtilizationSerialTotalAmount
		{
			get
			{
				if (SerialTotalAmount <= 0)
					return 0;

				return SerialRentAmount / SerialTotalAmount * 100;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public decimal UtilizationSerialTotalCount
		{
			get
			{
				if (SerialTotalCount <= 0)
					return 0;

				return SerialRentCount / SerialTotalCount * 100;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public decimal UnUtilizationPercentAmount
		{
			get
			{
				if (TotalAmount == 0)
					return 100;

				var val = 1 - (RentAmount / TotalAmount) * 100;

				if (val < 0)
					return 100;

				return val;
			}
		}
	}
}
