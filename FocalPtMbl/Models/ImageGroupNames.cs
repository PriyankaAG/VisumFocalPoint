using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPoint.Models
{
	public class ImageGroupNames
	{
		public static string Outgoing = "Outgoing";
		public static string Incoming = "Incoming";
		public static string Damage = "Damage";
		public static string Miscellaneous = "Miscellaneous";

		public static List<string> GetDisplayOrder()
		{
			return new List<string>() { Outgoing, Incoming, Damage, Miscellaneous };
		}

		public static short GetGroupNumber(string group)
		{
			if (group == Outgoing)
				return 2;
			if (group == Incoming)
				return 3;
			if (group == Damage)
				return 4;
			if (group == Miscellaneous)
				return 1;

			return 0;
		}

		public static string GetGroupName(int zeroBasedOrderedIndex)
		{
			if (zeroBasedOrderedIndex == 0)
				return Outgoing;
			if (zeroBasedOrderedIndex == 1)
				return Incoming;
			if (zeroBasedOrderedIndex == 2)
				return Damage;
			if (zeroBasedOrderedIndex == 3)
				return Miscellaneous;

			return string.Empty;
		}

		public static short ConvertZeroBasedIndexToGroupNumber(int zeroBasedOrderedIndex)
		{
			return GetGroupNumber(GetGroupName(zeroBasedOrderedIndex));
		}
	}
}
