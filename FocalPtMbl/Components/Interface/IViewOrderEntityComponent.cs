using FocalPoint.Components.Common;
using FocalPoint.Components.Common.Interface;
using FocalPoint.Data.API;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Components.EntityComponents
{
    public interface IViewOrderEntityComponent
    {
        Task<Order> GetOrderDetails(int orderNo);

        Task<SignatureMessageOutputDTO> GetSignatureMessageDTO(SignatureMessageInputDTO signatureMessageInputDTO);

        Task<bool> SaveSignature(SignatureInputDTO signatureInputDTO);

        Task<bool> SaveOrderImage(OrderImageInputDTO orderImageInputDTO);

        Task<List<OrderImageDetail>> GetOrderImages(string orderNo);
    }
}
