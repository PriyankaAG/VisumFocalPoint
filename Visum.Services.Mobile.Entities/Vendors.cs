using System.Collections.Generic;

namespace Visum.Services.Mobile.Entities
{
    public class Vendors
    {
        public List<Vendor> List { get; set; } = new List<Vendor>();
        public int TotalCnt { get; set; }
    }
}
