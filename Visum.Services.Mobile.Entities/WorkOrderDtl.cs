
namespace Visum.Services.Mobile.Entities
{
    public class WorkOrderDtl
    {
        public int WODtlNo { get; set; }
        public int WODtlLine { get; set; }
        public string WODtlType { get; set; }
        public string WODtlDscr { get; set; }
        public string WODtlMfg { get; set; }
        public string WODtlPart { get; set; }
        public decimal WODtlQty { get; set; }
        public decimal WODtlPrice { get; set; }
        public string WODtlStatus { get; set; }
        public string WODtlStatusText { get; set; }
    }
}
