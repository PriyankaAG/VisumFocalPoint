using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FocalPoint.Data.DataModel
{
    [Table("tblSubGroup")]
    public class SubGroup
    {
        [Indexed, Column("SubGroupID")]
        public int SubGroupID { get; set; }
        [Indexed, Column("SubGroupCmp")]
        public int SubGroupCmp { get; set; }
        [MaxLength(35), Column("SubGroupDscr")]
        public string SubGroupDscr { get; set; }
        [Column("SubGroupMinChg")]
        public decimal SubGroupMinChg { get; set; }
        [Column("SubGroupMinHrs")]
        public int SubGroupMinHrs { get; set; }
        [Column("SubGroupMinChg2")]
        public decimal SubGroupMinChg2 { get; set; }
        [Column("SubGroupMinHrs2")]
        public int SubGroupMinHrs2 { get; set; }
        [Column("SubGroupMinChg3")]
        public decimal SubGroupMinChg3 { get; set; }
        [Column("SubGroupMinHrs3")]
        public int SubGroupMinHrs3 { get; set; }
        [Column("SubGroupHourChg")]
        public decimal SubGroupHourChg { get; set; }
        [Column("SubGroupDayChg")]
        public decimal SubGroupDayChg { get; set; }
        [Column("SubGroupWeeklyChg")]
        public decimal SubGroupWeeklyChg { get; set; }
        [Column("SubGroupDaysInWeek")]
        public int SubGroupDaysInWeek { get; set; }
        [Column("SubGroupMonthChg")]
        public decimal SubGroupMonthChg { get; set; }
        [Column("SubGroupOverNightChg")]
        public decimal SubGroupOverNightChg { get; set; }
        [Column("SubGroupSatMonChg")]
        public decimal SubGroupSatMonChg { get; set; }
        [Column("SubGroupFriMonChg")]
        public decimal SubGroupFriMonChg { get; set; }
        [Column("SubGroupOpenToCloseChg")]
        public decimal SubGroupOpenToCloseChg { get; set; }
        [Column("SubGroupPack1Active")]
        public bool SubGroupPack1Active { get; set; }
        [MaxLength(50), Column("SubGroupPack1Dscr")]
        public string SubGroupPack1Dscr { get; set; }
        [Column("SubGroupPack1Day1Chg")]
        public decimal SubGroupPack1Day1Chg { get; set; }
        [Column("SubGroupPack1Day2Chg")]
        public decimal SubGroupPack1Day2Chg { get; set; }
        [Column("SubGroupPack1Day3Chg")]
        public decimal SubGroupPack1Day3Chg { get; set; }
        [Column("SubGroupPack1Day4Chg")]
        public decimal SubGroupPack1Day4Chg { get; set; }
        [Column("SubGroupPack1Day5Chg")]
        public decimal SubGroupPack1Day5Chg { get; set; }
        [Column("SubGroupPack1Day6Chg")]
        public decimal SubGroupPack1Day6Chg { get; set; }
        [Column("SubGroupPack1Day7Chg")]
        public decimal SubGroupPack1Day7Chg { get; set; }
        [Column("SubGroupPack2Active")]
        public bool SubGroupPack2Active { get; set; }
        [MaxLength(50), Column("SubGroupPack2Dscr")]
        public string SubGroupPack2Dscr { get; set; }
        [Column("SubGroupPack2Day1Chg")]
        public decimal SubGroupPack2Day1Chg { get; set; }
        [Column("SubGroupPack2Day2Chg")]
        public decimal SubGroupPack2Day2Chg { get; set; }
        [Column("SubGroupPack2Day3Chg")]
        public decimal SubGroupPack2Day3Chg { get; set; }
        [Column("SubGroupPack2Day4Chg")]
        public decimal SubGroupPack2Day4Chg { get; set; }
        [Column("SubGroupPack2Day5Chg")]
        public decimal SubGroupPack2Day5Chg { get; set; }
        [Column("SubGroupPack2Day6Chg")]
        public decimal SubGroupPack2Day6Chg { get; set; }
        [Column("SubGroupPack2Day7Chg")]
        public decimal SubGroupPack2Day7Chg { get; set; }
        [Column("SubGroupPack3Active")]
        public bool SubGroupPack3Active { get; set; }
        [MaxLength(50), Column("SubGroupPack3Dscr")]
        public string SubGroupPack3Dscr { get; set; }
        [Column("SubGroupPack3Day1Chg")]
        public decimal SubGroupPack3Day1Chg { get; set; }
        [Column("SubGroupPack3Day2Chg")]
        public decimal SubGroupPack3Day2Chg { get; set; }
        [Column("SubGroupPack3Day3Chg")]
        public decimal SubGroupPack3Day3Chg { get; set; }
        [Column("SubGroupPack3Day4Chg")]
        public decimal SubGroupPack3Day4Chg { get; set; }
        [Column("SubGroupPack3Day5Chg")]
        public decimal SubGroupPack3Day5Chg { get; set; }
        [Column("SubGroupPack3Day6Chg")]
        public decimal SubGroupPack3Day6Chg { get; set; }
        [Column("SubGroupPack3Day7Chg")]
        public decimal SubGroupPack3Day7Chg { get; set; }
        [Column("SubGroupLastUpdated")]
        public string SubGroupLastUpdated { get; set; }
        //public DateTime SubGroupLastUpdated { get; set; }
    }
}
