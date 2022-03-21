using FocalPoint.Components.Common;
using FocalPoint.Data.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint
{
    public class ViewOrderEntityComponent: IViewOrderEntityComponent
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
            try
            {
                HttpResponseMessage httpResponseMessage = await apiComponent.GetAsync(string.Format(OrderDetail, orderNo));
                if (httpResponseMessage?.IsSuccessStatusCode ?? false)
                {
                    string content = await httpResponseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<Order>(content);
                    return response;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<bool> SaveOrderImage(OrderImageInputDTO orderImageInputDTO)
        {
            string requestContent = JsonConvert.SerializeObject(orderImageInputDTO);
            HttpResponseMessage httpResponseMessage = await apiComponent.PostAsyc(orderImage, requestContent);
            if (httpResponseMessage?.IsSuccessStatusCode ?? false)
                return true;
            return false;
        }

        public async Task<List<OrderImageDetail>> GetOrderImages(string orderNo)
        {
            var url = string.Format(orderImages, orderNo);
            try
            {
                HttpResponseMessage httpResponseMessage = await apiComponent.GetAsync(url);
                if (httpResponseMessage?.IsSuccessStatusCode ?? false)
                {
                    string content = await httpResponseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<List<OrderImageDetail>>(content);
                    return response;
                }
            }
            catch(Exception ex)
            {

            }
            return null;
        }
    }
}
