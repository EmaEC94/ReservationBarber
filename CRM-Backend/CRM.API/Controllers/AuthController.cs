using CRM.Application.Dtos.User.Request;
using CRM.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthApplication _authApplication;
        public AuthController(IAuthApplication authApplication)
        {
            _authApplication = authApplication;
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] TokenRequestDto requestDto, [FromQuery] string authType)
        {
            var response = await _authApplication.Login(requestDto, authType);
            return Ok(response);
        }

        [HttpPost("LoginWithGoogle")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string credentials, [FromQuery] string authType)
        {
            var response = await _authApplication.LoginWithGoogle(credentials, authType);
            return Ok(response);
        }


        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset(ResetPasswordRequestDto requestDto)
        {
            var response = await _authApplication.RequestPasswordReset(requestDto);
            return Ok(response);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto requestDto)
        {
            var result = await _authApplication.ChangePassword(requestDto);
            return Ok(result);
        }
    }
}
