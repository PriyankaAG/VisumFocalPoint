using FocalPoint.Components.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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
        const string OrderSettingsAPIKey = "OrderSettings/";
        const string OrderAPIKey = "Order/";
        const string OrderAddRentalAPIKey = "OrderAddRental/";
        const string OrderAddMerchandiseAPIKey = "OrderAddMerchandise/";
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

        public async Task<OrderUpdate> GetNewOrderCreationDetail()
        {
            OrderUpdate orderUpdate = null;
            var Order = new Order();
            try
            {
                OrderSettings settings = await apiComponent.GetAsync<OrderSettings>(OrderSettingsAPIKey);
                SetDefaults(Order, settings);
                orderUpdate = await apiComponent.PostAsync<OrderUpdate>(OrderAPIKey, JsonConvert.SerializeObject(new { Order }));
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return orderUpdate;
        }

        public async Task<bool> VoidOrder(Order CurrentOrder)
        {
            try
            {
                if (CurrentOrder != null)
                {
                    CurrentOrder.OrderStatus = "V";
                    var Update = new OrderUpdate();
                    Update.Order = CurrentOrder;
                    return await apiComponent.PostAsync<bool>(OrderAPIKey, JsonConvert.SerializeObject(new { Update }));
                }
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return false;
        }

        public async Task<HttpResponseMessage> OrderAddRental(OrderAddItem RentalItem)
        {
            try
            {
                return await apiComponent.PostAsync(OrderAddRentalAPIKey, JsonConvert.SerializeObject(new { RentalItem }));
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return null;
        }

        public async Task<HttpResponseMessage> OrderAddMerchandise(OrderAddItem MerchItem)
        {
            try
            {
                return await apiComponent.PostAsync(OrderAddMerchandiseAPIKey, JsonConvert.SerializeObject(new { MerchItem }));
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return null;
        }

        private Order SetDefaults(Order newOrder, OrderSettings currentSettings)
        {
            newOrder.OrderCustNo = currentSettings.Defaults.OrderCustNo;
            newOrder.OrderDDte = currentSettings.Defaults.OrderDDte;
            newOrder.OrderEDays = currentSettings.Defaults.OrderEDays;
            newOrder.OrderEventRate = currentSettings.Defaults.OrderEventRate;
            newOrder.OrderLength = currentSettings.Defaults.OrderLength;
            newOrder.OrderODte = currentSettings.Defaults.OrderODte;
            newOrder.OrderTaxCode = currentSettings.Defaults.OrderTaxCode;
            newOrder.OrderType = currentSettings.Defaults.OrderType;
            return newOrder;
        }
    }
}
