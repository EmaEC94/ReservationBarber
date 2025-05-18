using CRM.Application.Commons.Bases.Request;
using CRM.Application.Commons.Bases.Response;
using CRM.Application.Commons.Select.Response;
using CRM.Application.Dtos.Client.Request;
using CRM.Application.Dtos.Client.Response;

namespace CRM.Application.Interfaces
{
    public interface IClientApplication
    {
        Task<BaseResponse<IEnumerable<ClientResponseDto>>> ListClient(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectClient();
        Task<BaseResponse<ClientByIdResponseDto>> ClientById(int clientId);
        Task<BaseResponse<bool>> RegisterClient(ClientRequestDto requestDto);
        Task<BaseResponse<bool>> EditClient(int clientId, ClientRequestDto requestDto);    
        Task<BaseResponse<bool>> RemoveClient(int clientId);
    }
}
