using FocalPoint.Data.API;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FocalPoint.Components.Interface
{
    public interface IGeneralComponent
    {
        Task<bool> SendEmailDocument(EmailDocumentInputDTO emailDocumentInputDTO);
    }
}
