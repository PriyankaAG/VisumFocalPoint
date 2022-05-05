using FocalPoint.Components.Interface;
using System;
using System.Threading.Tasks;

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

        public Task<> GetDashboardDetails(DateTime searchDate)
        {
            Guid result;
            try
            {
                result = await apiComponent.GetAsync<Guid>(string.Format(url, searchDate));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
