using System;
using System.Threading.Tasks;

namespace FocalPoint.Components.Interface
{
    public interface IFrontCounterEntityComponent
    {
        Task<> GetDashboardDetails(DateTime searchDate);
    }
}
