using FocalPoint.Data.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint
{
    public class ViewOrderEntityComponent : IViewOrderEntityComponent
    {
        IAPICompnent apiComponent;

        public const string OrderDetail = "Order/{0}";
        public const string orderImage = "OrderImage";
        public const string orderImages = "OrderImages/{0}";
        public ViewOrderEntityComponent()
        {
            apiComponent = new APIComponent();
        }        

        public async Task<Order> GetOrderDetails(int orderNo)
        {
            Order order = default;
            try
            {
                order = await apiComponent.GetAsync<Order>(string.Format(OrderDetail, orderNo));
            }
            catch (Exception ex)
            {

            }
            return order;
        }

        public async Task<bool> SaveOrderImage(OrderImageInputDTO orderImageInputDTO)
        {
            string requestContent = JsonConvert.SerializeObject(orderImageInputDTO);
            return await apiComponent.PostAsync<bool>(orderImage, requestContent);
        }

        public async Task<List<OrderImageDetail>> GetOrderImages(string orderNo)
        {
            List<OrderImageDetail> orderImageDetails = default;
            try
            {
                orderImageDetails = await apiComponent.GetAsync<List<OrderImageDetail>>(string.Format(orderImages, orderNo));
            }
            catch (Exception ex)
            {

            }
            return orderImageDetails;
        }
    }
}
