using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FocalPoint.Data.DataModel
{
    [Table("tblRental")]
    public class Rental
    {
        [PrimaryKey, Column("RentalItem")]
        public int RentalItem { get; set; }
        [Column("RentalStore")]
        public int RentalStore { get; set; }
        [Column("RentalBelongsTo")]
        public int RentalBelongsTo { get; set; }
        [MaxLength(35), Column("RentalDscr")]
        public string RentalDscr { get; set; }
        [MaxLength(25), Column("RentalEquipID")]
        public string RentalEquipID { get; set; }
        [Column("RentalSubGroup")]
        public int RentalSubGroup { get; set; }
        [Column("RentalYTDPct")]
        public decimal RentalYTDPct { get; set; }
        [Column("RentalLTDPct")]
        public decimal RentalLTDPct { get; set; }
        [Column("RentalYTDAmt")]
        public decimal RentalYTDAmt { get; set; }
        [Column("RentalLTDAmt")]
        public decimal RentalLTDAmt { get; set; }
        [Column("RentalYTDUnit")]
        public int RentalYTDUnit { get; set; }
        [Column("RentalLTDUnit")]
        public int RentalLTDUnit { get; set; }
        [Column("RentalFirstRented")]
        public string RentalFirstRented { get; set; }
        [Column("RentalLastRented")]
        public string RentalLastRented { get; set; }
        [Column("RentalRepairYTD")]
        public decimal RentalRepairYTD { get; set; }
        [Column("RentalRepairLTD")]
        public decimal RentalRepairLTD { get; set; }
        [Column("RentalCost")]
        public decimal RentalCost { get; set; }
        [Column("RentalRetail")]
        public decimal RentalRetail { get; set; }
        [MaxLength(50), Column("RentalMake")]
        public string RentalMake { get; set; }
        [MaxLength(50), Column("RentalModel")]
        public string RentalModel { get; set; }
        [Column("RentalYr")]
        public int? RentalYr { get; set; }
        [MaxLength(50), Column("RentalMfgName")]
        public string RentalMfgName { get; set; }
        [Column("RentalNote")]
        public string RentalNote { get; set; }
        [Column("RentalLastUpdated")]
        public string RentalLastUpdated { get; set; }

        [Ignore()]
        public bool IsSelected { get; set; }
    }
}
