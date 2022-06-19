using System;
using System.Collections.Generic;
using System.Text;
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

        Task<OrderUpdate> GetNewOrderCreationDetail(OrderSettings settings);

        Task<bool> VoidOrder(Order CurrentOrder);

        Task<OrderSettings> GetOrderSettings();

        Task AddCustomer(Customer custToAdd);
    }
}
