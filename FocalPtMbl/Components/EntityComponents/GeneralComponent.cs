using FocalPoint.Data.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint
{
    public class GeneralComponent : IGeneralComponent
    {
        IAPICompnent apiComponent; 
        public const string postEmailDocument = "Email/Document/";
        public const string postSignatureMessage = "Signature/Messages";
        public const string postSaveSignature = "Signature/Waiver";
        public const string getCountry = "Countries";
        public const string getStates = "States/{0}";

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

        public async Task<List<Country>> GetCountries()
        {
            List<Country> countries = null;
            try
            {
                countries = await apiComponent.GetAsync<List<Country>>(getCountry);
            }
            catch (Exception exception)
            {
                //TODO: Track Error
            }
            return countries;
        }

        public async Task<List<State>> GetStates(int countryCode)
        {
            List<State> states = null;
            try
            {
                states = await apiComponent.GetAsync<List<State>>(string.Format(getStates, countryCode));
            }
            catch (Exception exception)
            {
                //TODO: Track Error
            }
            return states;
        }

        public void HandleTokenExpired()
        {
            apiComponent.HandleTokenExpired();
        }
    }
}
