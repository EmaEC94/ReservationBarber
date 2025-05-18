using CRM.Application.Dtos.ActivePause.Request;
using CRM.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivePauseController : ControllerBase
    {
        private readonly IActivePauseApplication _activePauseApplication;

        public ActivePauseController(IActivePauseApplication activePauseApplication)
        {
            _activePauseApplication = activePauseApplication;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterActivePause([FromForm] ActivePauseRequestDto requestDto)
        {
            var response = await _activePauseApplication.RegisterActivePause(requestDto);
            return Ok(response);

        }
    }
}
