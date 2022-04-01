using FocalPoint.Components.Interface;
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
                //trucksList = await apiComponent.GetAsync<List<Truck>>(TruckAPIKey);

                trucksList = new List<Truck>()
                {
                    new Truck() { TruckID = "1000", TruckDscr = "Delivery Truck #2", TruckMapIcon = 1, TruckNo = 2 },
                    new Truck() { TruckID = "02", TruckDscr = "f450 Service Truck", TruckMapIcon = 6, TruckNo = 1 },
                    new Truck() { TruckID = "[ID]", TruckDscr = "Unassigned Truck", TruckMapIcon = 3, TruckNo = 3 }
                };

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
                //API Call
                dispatchList = await apiComponent.GetAsync<List<Dispatches>>(string.Format(DispatchesAPIKey, searchDate));

                dispatchList = new List<Dispatches>()
                {
                   new Dispatches(){ 
                       DispatchTruckID = 2,
                       DispatchDscr = "Hello World ONE",
                       DispatchAddr1 = "Address 1",
                       DispatchAddr2 = "Address 2",
                       DispatchCity = "Mumbai",
                       DispatchCmp = 10021,
                       DispatchID = 982,
                       DispatchSubject = "Delivery AAA",
                       DispatchStartDte = DateTime.Now,
                       OriginTimeNotes = "Origin Notes",
                       OriginContact = "Origin Contact",
                       OriginPhone = "(999)5655745",
                       ShipToContact = "Ship To Contact",
                       ShipToPhone = "3435676886",
                       OriginNotes = "Origin Notes"
                   },
                   new Dispatches(){ DispatchTruckID = 2, DispatchDscr = "Hello World TWO", DispatchAddr1 = "Address 2", DispatchAddr2 = "Address 2", DispatchCity = "Pune", DispatchCmp = 10041, DispatchID = 985, DispatchSubject = "Delivery BBB" },
                   new Dispatches(){ DispatchTruckID = 1, DispatchDscr = "Hello World THREE", DispatchAddr1 = "Address 3", DispatchAddr2 = "Address 2", DispatchCity = "Banglore", DispatchCmp = 10023, DispatchID = 932, DispatchSubject = "Delivery CCC" },
                   new Dispatches(){ DispatchTruckID = 2, DispatchDscr = "Hello World FOUR", DispatchAddr1 = "Address 4", DispatchAddr2 = "Address 2", DispatchCity = "Delhi", DispatchCmp = 10027, DispatchID = 111, DispatchSubject = "Delivery DDD" },
                   new Dispatches(){ DispatchTruckID = 2, DispatchDscr = "Hello World FIVE", DispatchAddr1 = "Address 5", DispatchAddr2 = "Address 2", DispatchCity = "Surat", DispatchCmp = 10066, DispatchID = 532, DispatchSubject = "Delivery EEE " },
                   new Dispatches(){ DispatchTruckID = 1, DispatchDscr = "Hello World SIX", DispatchAddr1 = "Address 6", DispatchAddr2 = "Address 2", DispatchCity = "Tirupati", DispatchCmp = 10233, DispatchID = 423, DispatchSubject = "Delivery FFF" }
                };

            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return dispatchList;
        }
    }
}
