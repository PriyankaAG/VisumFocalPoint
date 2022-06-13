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

    }
}
