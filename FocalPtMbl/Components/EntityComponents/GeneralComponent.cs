using FocalPoint.Components.Common;
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
        public const string postSignatureMessage = "Signature/Messages";
        public const string postSaveSignature = "Signature/Waiver";

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

        public async Task<SignatureMessageOutputDTO> GetSignatureMessageDTO(SignatureMessageInputDTO singnatureMessageInputDTO)
        {
            SignatureMessageOutputDTO singnatureMessageOutputDTO = null;
            string requestContent = JsonConvert.SerializeObject(singnatureMessageInputDTO);
            HttpResponseMessage httpResponseMessage = await apiComponent.PostAsyc(postSignatureMessage, requestContent);
            if (httpResponseMessage?.IsSuccessStatusCode ?? false)
            {
                string content = await httpResponseMessage.Content.ReadAsStringAsync();
                singnatureMessageOutputDTO = JsonConvert.DeserializeObject<SignatureMessageOutputDTO>(content);
            }
            return singnatureMessageOutputDTO;
        }

        public async Task<bool> SaveSignature(SignatureInputDTO signatureInputDTO)
        {
            string requestContent = JsonConvert.SerializeObject(signatureInputDTO);
            HttpResponseMessage httpResponseMessage = await apiComponent.PostAsyc(postSaveSignature, requestContent);
            if (httpResponseMessage?.IsSuccessStatusCode ?? false)
                return true;
            return false;
        }
    }
}
