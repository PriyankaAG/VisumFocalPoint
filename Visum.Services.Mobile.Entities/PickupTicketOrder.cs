
namespace Visum.Services.Mobile.Entities
{
    public class PickupTicketOrder
    {
        public int PuTNo { get; set; }
        public string OrderNumberT { get; set; }
        public int OrderNo { get; set; }
        public int OrderDtlNo { get; set; }
        public string OrderDtlDscr { get; set; }
        public string EquipID { get; set; }
        public int OrderDtlItem { get; set; }
        public short OrderDtlLine { get; set; }

        public decimal OrderDtlQty { get; set; }
        public decimal PuDtlQty { get; set; }
    }
}
