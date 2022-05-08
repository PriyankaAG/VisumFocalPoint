
namespace Visum.Services.Mobile.Entities
{
    public class WorkOrderTotals
    {
        public decimal TotalDueAmt { get; set; }
        public decimal TotalPartsCustAmt { get; set; }
        public decimal TotalPartsIntAmt { get; set; }
        public decimal TotalLaborCustAmt { get; set; }
        public decimal TotalLaborIntAmt { get; set; }
        public decimal TotalWarPartsCustAmt { get; set; }
        public decimal TotalWarPartsIntAmt { get; set; }
        public decimal TotalWarLaborCustAmt { get; set; }
        public decimal TotalWarLaborIntAmt { get; set; }
        public decimal TotalTaxCustAmt { get; set; }
        public decimal TotalTaxIntAmt { get; set; }
    }
}
