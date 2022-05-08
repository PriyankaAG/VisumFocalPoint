using System.Collections.Generic;

namespace Visum.Services.Mobile.Entities
{
	public class TimeClockUser
	{
		public string UserID { get; set; }

		public string FullName { get; set; }

		public int HomeStore { get; set; }

		public List<TimeClockStore> Stores { get; set; }
	}

}
