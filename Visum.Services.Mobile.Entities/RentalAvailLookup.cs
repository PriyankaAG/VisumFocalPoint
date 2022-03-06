using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace Visum.Services.Mobile.Entities
{
    [DataContract()]
    public class RentalAvailLookup
    {
        [DataMember]
        public int Type { get; set; }

        [DataMember]
        public List<RentalAvailTypes> Types { get; set; }

        public DateTime? StartDate { get; set; }
        [DataMember(Name = "StartDate")]
        private string strStartDate { get; set; }

        public DateTime? EndDate { get; set; }
        [DataMember(Name = "EndDate")]
        private string strEndDate { get; set; }

        [DataMember]
        public string Search { get; set; }

        [DataMember]
        public int Store { get; set; }

        [DataMember]
        public List<DisplayValueInteger > StoreList { get; set; }

        [OnSerializing]
        private void OnSerializing(StreamingContext ctx)
        {
            if (this.EndDate != null)
                this.strEndDate = this.EndDate.Value.ToString("g", CultureInfo.InvariantCulture);
            if (this.StartDate != null)
                this.strStartDate = this.StartDate.Value.ToString("g", CultureInfo.InvariantCulture);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            if (string.IsNullOrEmpty(this.strEndDate) == false)
                this.EndDate = DateTime.ParseExact(this.strEndDate, "g", CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(this.strStartDate) == false)
                this.StartDate = DateTime.ParseExact(this.strStartDate, "g", CultureInfo.InvariantCulture);
        }

        //public void LoadStores(IEnumerable<Store> dv)
        //{
        //    StoreList = new List<ComboItem>();
        //    foreach (Store store in dv)
        //    {
        //        ComboItem item = new ComboItem();
        //        item.ID = store.CmpNo;
        //        item.Name = store.CmpName;
        //        StoreList.Add(item);
        //    }
        //}
    }

    [DataContract()]
    public class RentalAvailTypes
    {
        [DataMember]
        public int TypeID { get; set; }

        [DataMember]
        public string TypeName { get; set; }
    }
}
