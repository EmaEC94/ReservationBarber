using CRM.Application.Commons.Bases.Request;
using CRM.Application.Commons.Bases.Response;
using CRM.Application.Dtos.ActivePause.Request;
using CRM.Application.Dtos.ActivePause.Response;

namespace CRM.Application.Interfaces
{
    public  interface IActivePauseApplication
    {
        Task<BaseResponse<IEnumerable<ActivePauseResponseDto>>> ListActivePause(BaseFiltersRequest filters);
        Task<BaseResponse<bool>> RegisterActivePause(ActivePauseRequestDto requestDto);


    }
}
