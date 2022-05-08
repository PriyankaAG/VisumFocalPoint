
namespace Visum.Services.Mobile.Entities
{
    public class CustomerBalance
    {
        public decimal ARBalance { get; set; }
        public decimal AR30 { get; set; }
        public decimal AR60 { get; set; }
        public decimal AR90 { get; set; }
        public decimal AR120 { get; set; }
        public decimal AROver120 { get; set; }
        public decimal ARUnapplied { get; set; }
    }
}
