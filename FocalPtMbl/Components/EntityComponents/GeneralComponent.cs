using FocalPoint.Data.API;
using Newtonsoft.Json;
using System;
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
            bool isEmailSuccess = default;
            try
            {
                string requestContent = JsonConvert.SerializeObject(emailDocumentInputDTO);
                isEmailSuccess = await apiComponent.PostAsync<bool>(postEmailDocument, requestContent);
            }
            catch (Exception exception)
            {
                //TODO: Track error
            }
            return isEmailSuccess;
        }

        public async Task<SignatureMessageOutputDTO> GetSignatureMessageDTO(SignatureMessageInputDTO singnatureMessageInputDTO)
        {
            SignatureMessageOutputDTO singnatureMessageOutputDTO = default;
            try
            {
                string requestContent = JsonConvert.SerializeObject(singnatureMessageInputDTO);
                singnatureMessageOutputDTO = await apiComponent.PostAsync<SignatureMessageOutputDTO>(postSignatureMessage, requestContent);
            }
            catch (Exception exception)
            {
                //TODO: Track Error
            }
            return singnatureMessageOutputDTO;
        }

        public async Task<bool> SaveSignature(SignatureInputDTO signatureInputDTO)
        {
            bool isSignatureSaved = default;
            try
            {
                string requestContent = JsonConvert.SerializeObject(signatureInputDTO);
                isSignatureSaved = await apiComponent.PostAsync<bool>(postSaveSignature, requestContent);
            }
            catch (Exception exception)
            {
                //TODO: Track Error
            }
            return isSignatureSaved;
        }
    }
}
