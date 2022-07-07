using FocalPoint.Components.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Components.EntityComponents
{
    public class NewQuickRentalEntityComponent : INewQuickRentalEntityComponent
    {
        IAPICompnent apiComponent;

        const string CustomerAPIKey = "Customers/";
        const string AddCustomerAPIKey = "Customer/";
        const string CustomerSettingsAPIKey = "CustomerSettings/";
        const string GetStatesAPIKey = "States/{0}";
        const string GetCitiesByStateAPIKey = "CitiesByState/{0}/{1}";
        const string OrderSettingsAPIKey = "OrderSettings/";
        const string OrderAPIKey = "Order/";
        const string OrderAddRentalAPIKey = "OrderAddRental/";
        const string OrderAddMerchandiseAPIKey = "OrderAddMerchandise/";
        const string AvailabilityRentalsAPIKey = "Availability/Rentals/";
        const string AvailabilityMerchandiseAPIKey = "Availability/Merchandise/";
        const string AvailabilityMerchandiseSerialsAPIKey = "AvailabilityMerchandiseSerials/{0}/{1}";
        const string OrderDetailAPIKey = "OrderDetail/";

        const string CheckPhoneNumberAPIKey = "CustomerPhoneDupes/{0}/{1}";
        const string CheckCustomerNameAPIKey = "CustomerNameCheck/{0}/{1}/{2}";
        const string CheckCustomerIDAPIKey = "CustomerIDCheck/{0}/{1}";

        private int StoreID = 1;
        private string Search = "%";
        private string OutDate = "2020-01-01T18:25:00.000";
        private string DueDate = "2020-01-01T18:25:00.000";
        private char SearchType = '0';
        private Int16 SearchFor = 1;

        public NewQuickRentalEntityComponent()
        {
            apiComponent = new APIComponent();
            //apiComponent.AddStoreToHeader("1");
            //apiComponent.AddTerminalToHeader("6");

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
        public async Task<CitiesStates> GetCityByState(string countryCode, string stateCode)
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

        public async Task<OrderSettings> GetOrderSettings()
        {
            OrderSettings settings = await apiComponent.GetAsync<OrderSettings>(OrderSettingsAPIKey);
            return settings;
        }
        public async Task<OrderUpdate> GetNewOrderCreationDetail(OrderSettings settings)
        {
            OrderUpdate orderUpdate = null;
            var Order = new Order();
            try
            {
                SetDefaults(Order, settings);
                var stringContent = new StringContent(
                       JsonConvert.SerializeObject(new { Order }),
                         Encoding.UTF8,
                         "application/json");
                string res = await stringContent.ReadAsStringAsync();
                orderUpdate = await apiComponent.PostAsync<OrderUpdate>(OrderAPIKey, res);
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

        public async Task<OrderUpdate> UpdateOrder(OrderUpdate Update)
        {
            OrderUpdate responseOrderUpdate = null;
            try
            {
                var stringContent = new StringContent(
            JsonConvert.SerializeObject(new { Update }),
              Encoding.UTF8,
              "application/json");
                string res = await stringContent.ReadAsStringAsync();
                responseOrderUpdate = await apiComponent.SendAsyncUpdateOrder(OrderAPIKey, res);

            }
            catch (Exception)
            {
            }

            return responseOrderUpdate;
        }
        public async Task<OrderDtlUpdate> UpdateOrderDetail(OrderDtlUpdate Update)
        {
            OrderDtlUpdate responseOrderDetailUpdate = null;
            try
            {
                var stringContent = new StringContent(
            JsonConvert.SerializeObject(new { Update }),
              Encoding.UTF8,
              "application/json");
                string res = await stringContent.ReadAsStringAsync();
                responseOrderDetailUpdate = await apiComponent.SendAsyncUpdateOrderDetails(OrderDetailAPIKey, res);

            }
            catch (Exception)
            {
            }

            return responseOrderDetailUpdate;
        }

        public async Task<List<AvailabilityRent>> GetAvailabilityRentals(string text, Int16 SearchIn)
        {
            List<AvailabilityRent> res = null;
            try
            {
                Search = text;
                res = await apiComponent.PostAsync<List<AvailabilityRent>>(AvailabilityRentalsAPIKey, JsonConvert.SerializeObject(new { Search, OutDate, DueDate, StoreID, SearchIn, SearchType, SearchFor }));
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return res;
        }       

        public async Task<List<AvailabilityMerch>> GetAvailabilityMerchandise(string text, Int16 SearchIn)
        {
            List<AvailabilityMerch> res = null;
            try
            {
                Search = text;
                res = await apiComponent.PostAsync<List<AvailabilityMerch>>(AvailabilityMerchandiseAPIKey, JsonConvert.SerializeObject(new { Search, SearchIn }));
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return res;
        }

        public async Task<List<string>> AvailabilityMerchandiseSerials(string merchNo, string storeNo)
        {
            List<string> res = null;
            try
            {
                res = await apiComponent.GetAsync<List<string>>(string.Format(AvailabilityMerchandiseSerialsAPIKey, merchNo, storeNo));
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return res;
        }

        public async Task<List<AvailabilityRent>> GetAvailabilityKits(string text, Int16 SearchIn)
        {
            List<AvailabilityRent> res = null;
            try
            {
                Search = text;
                res = await apiComponent.PostAsync<List<AvailabilityRent>>(AvailabilityRentalsAPIKey, JsonConvert.SerializeObject(new { Search, OutDate, DueDate, StoreID, SearchIn, SearchType, SearchFor }));
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return res;
        }

        public async Task<List<AvailabilityRent>> GetAvailabilitySalable(string text, Int16 SearchIn)
        {
            List<AvailabilityRent> res = null;
            try
            {
                Search = text;
                res = await apiComponent.PostAsync<List<AvailabilityRent>>(AvailabilityRentalsAPIKey, JsonConvert.SerializeObject(new { Search, OutDate, DueDate, StoreID, SearchIn, SearchType, SearchFor }));
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return res;
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
            if (currentSettings != null)
            {
                newOrder.OrderCustNo = currentSettings.Defaults.OrderCustNo;
                newOrder.OrderDDte = currentSettings.Defaults.OrderDDte;
                newOrder.OrderEDays = currentSettings.Defaults.OrderEDays;
                newOrder.OrderEventRate = currentSettings.Defaults.OrderEventRate;
                newOrder.OrderLength = currentSettings.Defaults.OrderLength;
                newOrder.OrderODte = currentSettings.Defaults.OrderODte;
                newOrder.OrderTaxCode = currentSettings.Defaults.OrderTaxCode;
                newOrder.OrderType = currentSettings.Defaults.OrderType;
            }
            return newOrder;
        }


        public async Task<Customer> AddCustomer(Customer NewCustomer)
        {
            Customer result = null;
            try
            {
                var stringContent = new StringContent(
                   JsonConvert.SerializeObject(new { NewCustomer }),
                     Encoding.UTF8,
                     "application/json");
                string res = await stringContent.ReadAsStringAsync();

                result = await apiComponent.PostAsync<Customer>(AddCustomerAPIKey, res);
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return result;
        }
        public async Task<string> CheckPhoneNumber(string phNumber)
        {
            string output = null;
            try
            {

                output = await apiComponent.GetAsync<string>(string.Format(CheckPhoneNumberAPIKey, 0, phNumber));

            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return output;
        }
        public async Task<string> CheckCustomerName(string FName, string LName)
        {
            string output = null;
            try
            {
                output = await apiComponent.GetAsync<string>(string.Format(CheckCustomerNameAPIKey, 0, FName, LName));

            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return output;
        }
        public async Task<string> CheckDrivLicID(string drivLicID)
        {
            string output = null;
            try
            {

                output = await apiComponent.GetAsync<string>(string.Format(CheckCustomerIDAPIKey, 0, drivLicID));

            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return output;
        }
    }
}
