using CRM.Application.Commons.Bases.Request;
using CRM.Application.Commons.Bases.Response;
using CRM.Application.Commons.Select.Response;
using CRM.Application.Dtos.User.Request;
using CRM.Application.Dtos.User.Response;

namespace CRM.Application.Interfaces
{
    public interface IUserApplication
    {
        Task<BaseResponse<IEnumerable<UserResponseDto>>> ListUsers(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectUser();
        Task<BaseResponse<UserByIdResponseDto>> UserById(int userId);
        Task<BaseResponse<bool>> RegisterUser(UserRequestDto requestDto);
        Task<BaseResponse<bool>> EditUser(int userId, UserRequestDto requestDto);
        Task<BaseResponse<bool>> RemoveUser(int userId);
    }
}
