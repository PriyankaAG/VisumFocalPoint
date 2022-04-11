using FocalPoint.Components.Interface;
using FocalPoint.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Components.EntityComponents
{
    public class DispatchingEntityComponent : IDispatchingEntityComponent
    {

        IAPICompnent apiComponent;

        const string TruckAPIKey = "Trucks/";
        const string DispatchesAPIKey = "Dispatching/Dispatches/{0}";

        public DispatchingEntityComponent()
        {
            apiComponent = new APIComponent();
        }

        public async Task<List<Truck>> GetTrucks()
        {
            List<Truck> trucksList = null;
            try
            {
                //API Call
                trucksList = await apiComponent.GetAsync<List<Truck>>(TruckAPIKey);

                #region Consume Using XML Response
                //try
                //{
                //    var httpResponseMessage = await apiComponent.GetAsync(TruckAPIKey);
                //    if (httpResponseMessage.IsSuccessStatusCode)
                //    {
                //        string content = await httpResponseMessage.Content.ReadAsStringAsync();
                //        if (content != null)
                //        {
                //            var indexOfFirstNode = content.IndexOf("<Truck>");
                //            var replace = content.Substring("<ArrayOfTruck ".Length, indexOfFirstNode - "<ArrayOfTruck ".Length - 1);
                //            content = content.Replace(replace, "");
                //        }
                //        trucksList = Serializer.Deserialize<List<Truck>>(content);
                //    }
                //}
                //catch (Exception ex)
                //{
                //} 
                #endregion

            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return trucksList;
        }
        public async Task<List<Dispatches>> GetDispatches(DateTime searchDate)
        {
            List<Dispatches> dispatchList = null;
            try
            {
                //2022 - 04 - 04T18: 25:43.511Z
                //UNIVERSAL TIME
                string dateToPass = searchDate.ToUniversalTime()
                         .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                //API Call
                dispatchList = await apiComponent.PostAsync<List<Dispatches>>(string.Format(DispatchesAPIKey, dateToPass), "");

                #region Commented Dummy Data
                //dispatchList = new List<Dispatches>()
                //{
                //   new Dispatches(){
                //       DispatchTruckID = 2,
                //       DispatchDscr = "Hello World ONE",
                //       DispatchAddr1 = "3840 US 42",
                //       DispatchAddr2 = "Florence, KY 41042",
                //       DispatchCity = "Mumbai",
                //       DispatchCmp = 10021,
                //       DispatchID = 982,
                //       DispatchSubject = "Delivery AAA",
                //       DispatchStartDte = DateTime.Now,
                //       OriginTimeNotes = "Origin Notes",
                //       OriginContact = "Origin Contact",
                //       OriginPhone = "(999)5655745",
                //       ShipToContact = "Ship To Contact",
                //       ShipToPhone = "(982)0596420",
                //       OriginNotes = "Origin Notes"
                //   },
                //    new Dispatches(){
                //       DispatchTruckID = 2,
                //       DispatchDscr = "Hello World ONE",
                //       DispatchAddr1 = "KDMC Office",
                //       DispatchAddr2 = "Kalyan, 421306",
                //       DispatchCity = "Mumbai",
                //       DispatchCmp = 10021,
                //       DispatchID = 982,
                //       DispatchSubject = "Delivery BBB",
                //       DispatchStartDte = DateTime.Now,
                //       OriginTimeNotes = "Origin Notes",
                //       OriginContact = "Origin Contact",
                //       OriginPhone = "(982)0596420",
                //       ShipToContact = "Ship To Contact",
                //       ShipToPhone = "(982)0596420",
                //       OriginNotes = "Origin Notes"
                //   },
                //   new Dispatches(){ DispatchTruckID = 2, DispatchDscr = "Hello World TWO", DispatchAddr1 = "Address 2", DispatchAddr2 = "Address 2", DispatchCity = "Pune", DispatchCmp = 10041, DispatchID = 985, DispatchSubject = "Delivery BBB" },
                //   new Dispatches(){ DispatchTruckID = 1, DispatchDscr = "Hello World THREE", DispatchAddr1 = "Address 3", DispatchAddr2 = "Address 2", DispatchCity = "Banglore", DispatchCmp = 10023, DispatchID = 932, DispatchSubject = "Delivery CCC" },
                //   new Dispatches(){ DispatchTruckID = 2, DispatchDscr = "Hello World FOUR", DispatchAddr1 = "Address 4", DispatchAddr2 = "Address 2", DispatchCity = "Delhi", DispatchCmp = 10027, DispatchID = 111, DispatchSubject = "Delivery DDD" },
                //   new Dispatches(){ DispatchTruckID = 2, DispatchDscr = "Hello World FIVE", DispatchAddr1 = "Address 5", DispatchAddr2 = "Address 2", DispatchCity = "Surat", DispatchCmp = 10066, DispatchID = 532, DispatchSubject = "Delivery EEE " },
                //   new Dispatches(){ DispatchTruckID = 1, DispatchDscr = "Hello World SIX", DispatchAddr1 = "Address 6", DispatchAddr2 = "Address 2", DispatchCity = "Tirupati", DispatchCmp = 10233, DispatchID = 423, DispatchSubject = "Delivery FFF" },
                //   new Dispatches(){ DispatchTruckID = 2, DispatchDscr = "Hello World TWO", DispatchAddr1 = "Address 2", DispatchAddr2 = "Address 2", DispatchCity = "Pune", DispatchCmp = 10041, DispatchID = 985, DispatchSubject = "Delivery BBB" },
                //   new Dispatches(){ DispatchTruckID = 1, DispatchDscr = "Hello World THREE", DispatchAddr1 = "Address 3", DispatchAddr2 = "Address 2", DispatchCity = "Banglore", DispatchCmp = 10023, DispatchID = 932, DispatchSubject = "Delivery CCC" },
                //   new Dispatches(){ DispatchTruckID = 4, DispatchDscr = "Hello World FOUR", DispatchAddr1 = "Address 4", DispatchAddr2 = "Address 2", DispatchCity = "Delhi", DispatchCmp = 10027, DispatchID = 111, DispatchSubject = "Delivery DDD" },
                //   new Dispatches(){ DispatchTruckID = 4, DispatchDscr = "Hello World FIVE", DispatchAddr1 = "Address 5", DispatchAddr2 = "Address 2", DispatchCity = "Surat", DispatchCmp = 10066, DispatchID = 532, DispatchSubject = "Delivery EEE " },
                //   new Dispatches(){ DispatchTruckID = 2, DispatchDscr = "Hello World TWO", DispatchAddr1 = "Address 2", DispatchAddr2 = "Address 2", DispatchCity = "Pune", DispatchCmp = 10041, DispatchID = 985, DispatchSubject = "Delivery BBB" },
                //   new Dispatches(){ DispatchTruckID = 1, DispatchDscr = "Hello World THREE", DispatchAddr1 = "Address 3", DispatchAddr2 = "Address 2", DispatchCity = "Banglore", DispatchCmp = 10023, DispatchID = 932, DispatchSubject = "Delivery CCC" },
                //   new Dispatches(){ DispatchTruckID = 2, DispatchDscr = "Hello World FOUR", DispatchAddr1 = "Address 4", DispatchAddr2 = "Address 2", DispatchCity = "Delhi", DispatchCmp = 10027, DispatchID = 111, DispatchSubject = "Delivery DDD" },
                //   new Dispatches(){ DispatchTruckID = 2, DispatchDscr = "Hello World FIVE", DispatchAddr1 = "Address 5", DispatchAddr2 = "Address 2", DispatchCity = "Surat", DispatchCmp = 10066, DispatchID = 532, DispatchSubject = "Delivery EEE " },
                //   new Dispatches(){ DispatchTruckID = 1, DispatchDscr = "Hello World SIX", DispatchAddr1 = "Address 6", DispatchAddr2 = "Address 2", DispatchCity = "Tirupati", DispatchCmp = 10233, DispatchID = 423, DispatchSubject = "Delivery FFF" },
                //   new Dispatches(){ DispatchTruckID = 2, DispatchDscr = "Hello World TWO", DispatchAddr1 = "Address 2", DispatchAddr2 = "Address 2", DispatchCity = "Pune", DispatchCmp = 10041, DispatchID = 985, DispatchSubject = "Delivery BBB" },
                //   new Dispatches(){ DispatchTruckID = 1, DispatchDscr = "Hello World THREE", DispatchAddr1 = "Address 3", DispatchAddr2 = "Address 2", DispatchCity = "Banglore", DispatchCmp = 10023, DispatchID = 932, DispatchSubject = "Delivery CCC" },
                //   new Dispatches(){ DispatchTruckID = 4, DispatchDscr = "Hello World FOUR", DispatchAddr1 = "Address 4", DispatchAddr2 = "Address 2", DispatchCity = "Delhi", DispatchCmp = 10027, DispatchID = 111, DispatchSubject = "Delivery DDD" },
                //   new Dispatches(){ DispatchTruckID = 4, DispatchDscr = "Hello World FIVE", DispatchAddr1 = "Address 5", DispatchAddr2 = "Address 2", DispatchCity = "Surat", DispatchCmp = 10066, DispatchID = 532, DispatchSubject = "Delivery EEE " },
                //   new Dispatches(){ DispatchTruckID = 2, DispatchDscr = "Hello World TWO", DispatchAddr1 = "Address 2", DispatchAddr2 = "Address 2", DispatchCity = "Pune", DispatchCmp = 10041, DispatchID = 985, DispatchSubject = "Delivery BBB" },
                //   new Dispatches(){ DispatchTruckID = 1, DispatchDscr = "Hello World THREE", DispatchAddr1 = "Address 3", DispatchAddr2 = "Address 2", DispatchCity = "Banglore", DispatchCmp = 10023, DispatchID = 932, DispatchSubject = "Delivery CCC" },
                //   new Dispatches(){ DispatchTruckID = 2, DispatchDscr = "Hello World FOUR", DispatchAddr1 = "Address 4", DispatchAddr2 = "Address 2", DispatchCity = "Delhi", DispatchCmp = 10027, DispatchID = 111, DispatchSubject = "Delivery DDD" },
                //   new Dispatches(){ DispatchTruckID = 2, DispatchDscr = "Hello World FIVE", DispatchAddr1 = "Address 5", DispatchAddr2 = "Address 2", DispatchCity = "Surat", DispatchCmp = 10066, DispatchID = 532, DispatchSubject = "Delivery EEE " },
                //   new Dispatches(){ DispatchTruckID = 1, DispatchDscr = "Hello World SIX", DispatchAddr1 = "Address 6", DispatchAddr2 = "Address 2", DispatchCity = "Tirupati", DispatchCmp = 10233, DispatchID = 423, DispatchSubject = "Delivery FFF" },
                //   new Dispatches(){ DispatchTruckID = 2, DispatchDscr = "Hello World TWO", DispatchAddr1 = "Address 2", DispatchAddr2 = "Address 2", DispatchCity = "Pune", DispatchCmp = 10041, DispatchID = 985, DispatchSubject = "Delivery BBB" },
                //   new Dispatches(){ DispatchTruckID = 1, DispatchDscr = "Hello World THREE", DispatchAddr1 = "Address 3", DispatchAddr2 = "Address 2", DispatchCity = "Banglore", DispatchCmp = 10023, DispatchID = 932, DispatchSubject = "Delivery CCC" },
                //   new Dispatches(){ DispatchTruckID = 4, DispatchDscr = "Hello World FOUR", DispatchAddr1 = "Address 4", DispatchAddr2 = "Address 2", DispatchCity = "Delhi", DispatchCmp = 10027, DispatchID = 111, DispatchSubject = "Delivery DDD" },
                //   new Dispatches(){ DispatchTruckID = 4, DispatchDscr = "Hello World FIVE", DispatchAddr1 = "Address 5", DispatchAddr2 = "Address 2", DispatchCity = "Surat", DispatchCmp = 10066, DispatchID = 532, DispatchSubject = "Delivery EEE " },
                //   new Dispatches(){ DispatchTruckID = 4, DispatchDscr = "Hello World SIX", DispatchAddr1 = "Address 6", DispatchAddr2 = "Address 2", DispatchCity = "Tirupati", DispatchCmp = 10233, DispatchID = 423, DispatchSubject = "Delivery FFF" },
                //   new Dispatches(){ DispatchTruckID = 4, DispatchDscr = "Hello World TWO", DispatchAddr1 = "Address 2", DispatchAddr2 = "Address 2", DispatchCity = "Pune", DispatchCmp = 10041, DispatchID = 985, DispatchSubject = "Delivery BBB" },
                //   new Dispatches(){ DispatchTruckID = 5, DispatchDscr = "Hello World THREE", DispatchAddr1 = "Address 3", DispatchAddr2 = "Address 2", DispatchCity = "Banglore", DispatchCmp = 10023, DispatchID = 932, DispatchSubject = "Delivery CCC" },
                //   new Dispatches(){ DispatchTruckID = 5, DispatchDscr = "Hello World FOUR", DispatchAddr1 = "Address 4", DispatchAddr2 = "Address 2", DispatchCity = "Delhi", DispatchCmp = 10027, DispatchID = 111, DispatchSubject = "Delivery DDD" },
                //   new Dispatches(){ DispatchTruckID = 5, DispatchDscr = "Hello World FIVE", DispatchAddr1 = "Address 5", DispatchAddr2 = "Address 2", DispatchCity = "Surat", DispatchCmp = 10066, DispatchID = 532, DispatchSubject = "Delivery EEE " },
                //   new Dispatches(){ DispatchTruckID = 5, DispatchDscr = "Hello World SIX", DispatchAddr1 = "Address 6", DispatchAddr2 = "Address 2", DispatchCity = "Tirupati", DispatchCmp = 10233, DispatchID = 423, DispatchSubject = "Delivery FFF" },
                //   new Dispatches(){ DispatchTruckID = 5, DispatchDscr = "Hello World TWO", DispatchAddr1 = "Address 2", DispatchAddr2 = "Address 2", DispatchCity = "Pune", DispatchCmp = 10041, DispatchID = 985, DispatchSubject = "Delivery BBB" },
                //   new Dispatches(){ DispatchTruckID = 5, DispatchDscr = "Hello World THREE", DispatchAddr1 = "Address 3", DispatchAddr2 = "Address 2", DispatchCity = "Banglore", DispatchCmp = 10023, DispatchID = 932, DispatchSubject = "Delivery CCC" },
                //   new Dispatches(){ DispatchTruckID = 6, DispatchDscr = "Hello World FOUR", DispatchAddr1 = "Address 4", DispatchAddr2 = "Address 2", DispatchCity = "Delhi", DispatchCmp = 10027, DispatchID = 111, DispatchSubject = "Delivery DDD" },
                //   new Dispatches(){ DispatchTruckID = 6, DispatchDscr = "Hello World FIVE", DispatchAddr1 = "Address 5", DispatchAddr2 = "Address 2", DispatchCity = "Surat", DispatchCmp = 10066, DispatchID = 532, DispatchSubject = "Delivery EEE " },
                //   new Dispatches(){ DispatchTruckID = 6, DispatchDscr = "Hello World SIX", DispatchAddr1 = "Address 6", DispatchAddr2 = "Address 2", DispatchCity = "Tirupati", DispatchCmp = 10233, DispatchID = 423, DispatchSubject = "Delivery FFF" }
                //}; 
                #endregion

            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return dispatchList;
        }
    }
}
