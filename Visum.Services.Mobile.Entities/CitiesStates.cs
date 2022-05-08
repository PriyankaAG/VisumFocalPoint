using System.Collections.Generic;

namespace Visum.Services.Mobile.Entities
{
    public class CitiesStates
    {
        public List<string> Cities { get; set; }
        public DisplayValueString State { get; set; }
        public int CodeID { get; set; }
    }
}
