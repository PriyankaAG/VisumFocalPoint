using FocalPoint.Components.Common;
using FocalPoint.Components.Common.Interface;
using FocalPoint.Components.Interface;
using FocalPoint.Data.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FocalPoint.Components.EntityComponents
{
    public class GeneralComponent : IGeneralComponent
    {
        IAPICompnent apiComponent; 
        public const string postEmailDocument = "Email/Document/";

        public GeneralComponent()
        {
            apiComponent = new APIComponent();
        }

        public async Task<bool> SendEmailDocument(EmailDocumentInputDTO emailDocumentInputDTO)
        {
            string requestContent = JsonConvert.SerializeObject(emailDocumentInputDTO);
            HttpResponseMessage httpResponseMessage = await apiComponent.PostAsyc(postEmailDocument, requestContent);
            if (httpResponseMessage?.IsSuccessStatusCode ?? false)
                return true;
            return false;
        }
    }
}
