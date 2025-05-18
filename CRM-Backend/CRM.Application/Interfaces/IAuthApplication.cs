using CRM.Application.Commons.Bases.Response;
using CRM.Application.Dtos.User.Request;

namespace CRM.Application.Interfaces
{
    public interface IAuthApplication
    {
        Task<BaseResponse<string>> Login(TokenRequestDto requestDto, string authType);
        Task<BaseResponse<string>> LoginWithGoogle(string credentials, string authType);
        Task<BaseResponse<bool>> RequestPasswordReset(ResetPasswordRequestDto requestDto);
        Task<BaseResponse<bool>> ChangePassword(ChangePasswordRequestDto requestDto);

    }
}
