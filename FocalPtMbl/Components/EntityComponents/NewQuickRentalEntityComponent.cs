using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.Components.Interface;
using Newtonsoft.Json;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Components.EntityComponents
{
    public class NewQuickRentalEntityComponent : INewQuickRentalEntityComponent
    {
        IAPICompnent apiComponent;

        const string CustomerAPIKey = "Customers/";
        const string CustomerSettingsAPIKey = "CustomerSettings/";
        const string GetStatesAPIKey = "States/{0}";
        const string GetCitiesByStateAPIKey = "CitiesByState/{0}/{1}";
        public NewQuickRentalEntityComponent()
        {
            apiComponent = new APIComponent();
        }

        public async Task<List<Customer>> GetCustomers(string searchCustomer)
        {
            List<Customer> custList = null;
            try
            {
                //API Call
                //custList = await apiComponent.GetAsync<List<Customer>>(CustomerAPIKey);

                string requestContent = JsonConvert.SerializeObject(new { SearchText = searchCustomer, StartIdx = 0, MaxCnt = 100 });
                var CList = await apiComponent.PostAsync<Customers>(CustomerAPIKey, requestContent);
                custList = CList.List;

            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return custList;
        }
        public async Task<CustomerSettings> GetCustomerSettings()
        {
            CustomerSettings settings = null;
            try
            {

                settings = await apiComponent.GetAsync<CustomerSettings>(CustomerSettingsAPIKey);
                //await Task.Delay(10000);
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return settings;
        }
        public async Task<List<DisplayValueString>> GetStates(string countryCode)
        {
            List<DisplayValueString> res = null;
            try
            {

                res = await apiComponent.GetAsync<List<DisplayValueString>>(string.Format(GetStatesAPIKey, countryCode));
                //await Task.Delay(10000);
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return res;
        }
        public async Task<CitiesStates> GetCityByState(string countryCode,string stateCode)
        {
            CitiesStates res = null;
            try
            {

                res = await apiComponent.GetAsync<CitiesStates>(string.Format(GetCitiesByStateAPIKey, countryCode, stateCode));
                //await Task.Delay(10000);
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return res;
        }
    }
}
