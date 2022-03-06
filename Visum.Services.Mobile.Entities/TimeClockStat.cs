
namespace Visum.Services.Mobile.Entities
{
    public class TimeClockStat
    {
        public string UserId { get; set; }
        public bool ClockedIn { get; set; }
        public int StoreNo { get; set; }
        public string Status { get; set; }
        public string TodayActivity { get; set; }
        public string LastActivity { get; set; }
        public int LastClockID { get; set; }
        public string ThisWeek { get; set; }
        public string LastWeek { get; set; }
        public string FirstDayOfWeek { get; set; }
    }
}
