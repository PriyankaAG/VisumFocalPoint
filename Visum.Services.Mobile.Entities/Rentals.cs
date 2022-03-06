using System.Collections.Generic;

namespace Visum.Services.Mobile.Entities
{
    public class Rentals
    {
        public int TotalCnt { get; set; }
        public List<Rental> List { get; set; } = new List<Rental>();
    }
}
