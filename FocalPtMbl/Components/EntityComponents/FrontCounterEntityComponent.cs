using FocalPoint.Components.Interface;
using System;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Components.EntityComponents
{
    public class FrontCounterEntityComponent: IFrontCounterEntityComponent
    {
        IAPICompnent apiComponent;

        const string GetDashboard = "Order/Dashboard/{0}/{1}";

        public FrontCounterEntityComponent()
        {
            apiComponent = new APIComponent();
        }

        public async Task<OrderDashboard> GetDashboardDetails(int storeNo, DateTime searchDate)
        {
            OrderDashboard orderDashboardDetail;
            try
            {
                string dateToPass = searchDate.ToUniversalTime()
                         .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                orderDashboardDetail = await apiComponent.GetAsync<OrderDashboard>(string.Format(GetDashboard, storeNo, dateToPass));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return orderDashboardDetail;
        }
    }
}
