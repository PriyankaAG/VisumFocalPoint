using FocalPoint.Components.Interface;
using System;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Components.EntityComponents
{
    public class FrontCounterEntityComponent: IFrontCounterEntityComponent
    {
        IAPICompnent apiComponent;

        const string GetDashboard = "Order/Dashboard/{0}";

        public FrontCounterEntityComponent()
        {
            apiComponent = new APIComponent();
        }

        public async Task<OrderDashboard> GetDashboardDetails(DateTime searchDate)
        {
            OrderDashboard orderDashboardDetail;
            try
            {
                orderDashboardDetail = await apiComponent.GetAsync<OrderDashboard>(string.Format(GetDashboard, searchDate));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return orderDashboardDetail;
        }
    }
}
