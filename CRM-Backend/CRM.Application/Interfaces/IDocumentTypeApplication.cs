using CRM.Application.Commons.Bases.Response;
using CRM.Application.Dtos.DocumentType.Resposne;

namespace CRM.Application.Interfaces
{
    public interface IDocumentTypeApplication
    {
        Task<BaseResponse<IEnumerable<DocumentTypeResponseDto>>> ListDocumentTypes();
    }
}
