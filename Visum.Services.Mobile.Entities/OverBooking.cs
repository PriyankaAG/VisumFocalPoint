
namespace Visum.Services.Mobile.Entities
{
    public class OverBooking
    {
        public int RentalItem { get; set; }
        public string RentalEquipID { get; set; }
        public string RentalDscr { get; set; }
        public int OrderCmp { get; set; }
        public int RentalQty { get; set; }
        public int OverBookQty { get; set; }
        public int RentalSubGroup { get; set; }
        public int OnRent { get; set; }
    }
}
