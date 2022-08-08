
using System.Linq;

namespace Visum.Services.Mobile.Entities
{
    public class Vendor
    {
        public int VenNo { get; set; }
        public string VenName { get; set; }
        public string VenAddr1 { get; set; }
        public string VenAddr2 { get; set; }
        public string VenCity { get; set; }
        public string VenZip { get; set; }
        public string VenState { get; set; }
        public string VenLink { get; set; }
        public string VenContact { get; set; }
        public string VenEMail { get; set; }
        public string VenPhone { get; set; }
        public string VenExt { get; set; }
        public string VenFax { get; set; }
        public string VenAcctNo { get; set; }
        public decimal VenLimit { get; set; }
        public decimal VenMinPO { get; set; }
        public string VenNotes { get; set; }
        public string VenAddress
        {
            get
            {
                var res = string.Join(", ", new string[] { VenAddr1, VenCity, VenState, VenZip }.Where(x => !string.IsNullOrWhiteSpace(x)));
                return string.IsNullOrWhiteSpace(res) ? null : res;
            }
        }
    }
}
