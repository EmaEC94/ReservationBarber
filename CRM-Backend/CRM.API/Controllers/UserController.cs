using CRM.Application.Commons.Bases.Request;
using CRM.Application.Dtos.User.Request;
using CRM.Application.Interfaces;
using CRM.Utilities.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserApplication _userApplication;
        private readonly IGenerateExcelApplicattion _generateExcelApplicattion;
        public UserController(IUserApplication userApplication, IGenerateExcelApplicattion generateExcelApplicattion)
        {
            _userApplication = userApplication;
            _generateExcelApplicattion = generateExcelApplicattion;
        }
        [HttpGet]
        public async Task<IActionResult> ListUsers([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _userApplication.ListUsers(filters);
            if (filters.Download.HasValue && filters.Download.Value)
            {
                var columnNames = ExcelColumnNames.GetColumnsUsers();
                var fileBytes = _generateExcelApplicattion.GenerateToExcel(response.Data!, columnNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }
            return Ok(response);
        }
        [HttpGet("Select")]
        public async Task<IActionResult> ListSelectUsers()
        {
            var response = await _userApplication.ListSelectUser();
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromForm] UserRequestDto requestDto)
        {
            var response = await _userApplication.RegisterUser(requestDto);
            return Ok(response);
        }

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> userById(int userId)
        {
            var response = await _userApplication.UserById(userId);
            return Ok(response);
        }
        [HttpPut("Edit/{userId}")]
        public async Task<IActionResult> EditUser(int userId, [FromForm] UserRequestDto requestDto)
        {
            var response = await _userApplication.EditUser(userId, requestDto);
            return Ok(response);
        }
        [HttpPut("Remove/{userId:int}")]
        public async Task<IActionResult> RemoveUser(int userId)
        {
            var response = await _userApplication.RemoveUser(userId);
            return Ok(response);
        }
    }
}
