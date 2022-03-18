using FocalPoint.Data.API;
using System.Collections.Generic;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint
{
    public interface IViewOrderEntityComponent
    {
        Task<Order> GetOrderDetails(int orderNo);

        Task<bool> SaveOrderImage(OrderImageInputDTO orderImageInputDTO);

        Task<List<OrderImageDetail>> GetOrderImages(string orderNo);
    }
}
