using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Components.Interface
{
    public interface INewQuickRentalEntityComponent
    {
        Task<List<Customer>> GetCustomers(string searchCustomer);

        Task<CustomerSettings> GetCustomerSettings();
        Task<StoreSettings> GetStoreSettings();
        Task<List<DisplayValueString>> GetStates(string countryCode);
        Task<string> GetOrderLock(int OrderNo, string OrderNumT, bool Locked);
        Task<CitiesStates> GetCityByState(string countryCode, string stateCode);

        Task<OrderUpdate> GetNewOrderCreationDetail(OrderSettings settings);

        Task<string> VoidOrder(Order CurrentOrder);

        Task<List<AvailabilityRent>> GetAvailabilityRentals(string text, Int16 SearchIn, string SearchType, int SearchFor);

        Task<List<AvailabilityMerch>> GetAvailabilityMerchandise(string text, Int16 searchIn);

        Task<List<MerchandiseSerial>> AvailabilityMerchandiseSerials(string merchNo, string storeNo);

        Task<List<AvailabilityKit>> GetAvailabilityKits(string text, Int16 SearchIn, string SearchType);

        Task<List<AvailabilityRentSale>> GetAvailabilitySalable(string text, Int16 SearchIn);

        Task<List<AvailabilityKit>> GetOrderKits(int OrderDtlNo);

        Task<List<AvailabilityKit>> OrderKitDetails(int OrderDtlNo);

        Task<HttpResponseMessage> OrderAddRental(OrderAddItem RentalItem);

        Task<HttpResponseMessage> OrderAddMerchandise(OrderAddItem RentalItem);

        Task<OrderSettings> GetOrderSettings();

        Task<Customer> AddCustomer(Customer custToAdd);

        Task<string> CheckPhoneNumber(string phNumber);

        Task<string> CheckCustomerName(string FName, string LName);

        Task<string> CheckDrivLicID(string drivLicID);
        Task<OrderUpdate> UpdateOrder(OrderUpdate Update);

        Task<OrderUpdate> UpdateOrderDetail(OrderDtlUpdate OrderDetailUpdate);
    }
}
