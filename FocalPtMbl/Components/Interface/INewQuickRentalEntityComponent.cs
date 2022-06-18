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

        Task<List<DisplayValueString>> GetStates(string countryCode);

        Task<CitiesStates> GetCityByState(string countryCode, string stateCode);

        Task<OrderUpdate> GetNewOrderCreationDetail();

        Task<bool> VoidOrder(Order CurrentOrder);

        Task<HttpResponseMessage> OrderAddRental(OrderAddItem RentalItem);

        Task<HttpResponseMessage> OrderAddMerchandise(OrderAddItem RentalItem);

    }
}
