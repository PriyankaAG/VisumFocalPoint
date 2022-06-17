using System;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Components.Interface
{
    public interface IFrontCounterEntityComponent
    {
        Task<OrderDashboard> GetDashboardDetails(int storeNo, DateTime searchDate);
    }
}