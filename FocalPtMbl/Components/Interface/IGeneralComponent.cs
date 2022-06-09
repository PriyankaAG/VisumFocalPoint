using FocalPoint.Data.API;
using System.Collections.Generic;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint
{
    public interface IGeneralComponent
    {
        Task<bool> SendEmailDocument(EmailDocumentInputDTO emailDocumentInputDTO);

        Task<SignatureMessageOutputDTO> GetSignatureMessageDTO(SignatureMessageInputDTO signatureMessageInputDTO);

        Task<bool> SaveSignature(SignatureInputDTO signatureInputDTO);

        Task<List<Country>> GetCountries();

        Task<List<State>> GetStates(int countryCode);
    }
}
