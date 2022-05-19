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

            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return dispatchList;
        }
    }
}
