using FocalPoint.Components.Common.Interface;
using FocalPoint.Data.API;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace FocalPoint
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
