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
        const string OrderSettingsAPIKey = "OrderSettings/";
        const string OrderAPIKey = "Order/";
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

        public async Task<OrderUpdate> GetNewOrderCreationDetail()
        {
            OrderUpdate orderUpdate = null;
            var order = new Order();
            try
            {

                OrderSettings settings = await apiComponent.GetAsync<OrderSettings>(OrderSettingsAPIKey);
                SetDefaults(order, settings);
                orderUpdate = await apiComponent.PostAsync<OrderUpdate>(OrderAPIKey, JsonConvert.SerializeObject(new { order }));
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return orderUpdate;
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
