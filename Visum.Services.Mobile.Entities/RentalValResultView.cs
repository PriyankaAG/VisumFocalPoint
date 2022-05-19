
namespace Visum.Services.Mobile.Entities
{
    public class RentalValResultView
    {
        public decimal TotalAmt { get; set; }
        public int TotalCount { get; set; }
        public decimal RentAmount { get; set; }
        public int RentCount { get; set; }
        public decimal OffRentAmount { get; set; }
        public int OffRentCount { get; set; }
        public decimal SerialTotalAmount { get; set; }
        public int SerialTotalCount { get; set; }
        public decimal SerialRentAmount { get; set; }
        public int SerialRentCount { get; set; }

        public decimal SerialOffRentAmount { get; set; }

        public int SerialOffRentCount { get; set; }
        public decimal UtilizationPercentAmount { get; set; }

        public decimal UtilizationPercentCount { get; set; }
        public decimal UtilizationSerialTotalAmount { get; set; }
        public decimal UtilizationSerialTotalCount { get; set; }
        public decimal UnUtilizationPercentAmount { get; set; }
    }
}
