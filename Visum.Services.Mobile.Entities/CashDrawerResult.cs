
namespace Visum.Services.Mobile.Entities
{
    public class CashDrawerResult
    {
        public decimal CashDSAmt { get; set; }
        public decimal CashDEAmt { get; set; }
        public decimal CashDDeposit { get; set; }
        public short DepAcct { get; set; }
        public string DepAcctName { get; set; }
        public decimal CashDCashIn { get; set; }
        public decimal CashDCashOut { get; set; }
        public decimal CashDCheckIn { get; set; }
        public decimal CheckOut { get; set; }
        public decimal CashDCheckOut { get; set; }
        public decimal CashDCCOut { get; set; }
        public decimal CashDOtherIn { get; set; }
        public decimal CashDOtherOut { get; set; }
        public decimal RefundIn { get; set; }
        public decimal RefundOut { get; set; }
    }
}
