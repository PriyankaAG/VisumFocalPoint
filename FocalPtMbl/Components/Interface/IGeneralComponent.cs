using FocalPoint.Data.API;
using System.Threading.Tasks;

namespace FocalPoint
{
    public interface IGeneralComponent
    {
        Task<bool> SendEmailDocument(EmailDocumentInputDTO emailDocumentInputDTO);

        Task<SignatureMessageOutputDTO> GetSignatureMessageDTO(SignatureMessageInputDTO signatureMessageInputDTO);

        Task<bool> SaveSignature(SignatureInputDTO signatureInputDTO);
    }
}
