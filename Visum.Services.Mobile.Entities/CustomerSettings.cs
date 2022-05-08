using System.Collections.Generic;

namespace Visum.Services.Mobile.Entities
{
    public class CustomerSettings
    {
        public CustomerQuickEditDefaults Defaults { get; set; } = new CustomerQuickEditDefaults();
        public CustomerQuickEditControls Controls { get; set; } = new CustomerQuickEditControls();
        public List<DisplayValueString> CustomerStatus { get; set; } = new List<DisplayValueString>();
        public List<DisplayValueShort> CustomerCTypes { get; set; } = new List<DisplayValueShort>();
        public List<DisplayValueShort> CustomerTerms { get; set; } = new List<DisplayValueShort>();
        public List<DisplayValueInteger> CustomerFounds { get; set; } = new List<DisplayValueInteger>();
        public List<DisplayValueString> CustomerTypes { get; set; } = new List<DisplayValueString>();
        public List<DisplayValueShort> Countrys { get; set; } = new List<DisplayValueShort>();
    }

    public class CustomerQuickEditControls
    {
        public bool CustomerCTypeReq { get; set; }
        public bool AllowChangeCustomerType { get; set; }
        public bool FormatLastNameFirst { get; set; }
    }

    public class CustomerQuickEditDefaults
    {
        public int CustomerStore { get; set; }
        public string CustomerType { get; set; }
        public short CustomerTerms { get; set; }
        public bool CustomerDW { get; set; }
        public string CustomerStatus { get; set; }
        public string CustomerPhoneType { get; set; }
        public string CustomerPhoneType2 { get; set; }
        public string CustomerPhoneType3 { get; set; }
        public short CustomerCountry { get; set; }
    }

}
