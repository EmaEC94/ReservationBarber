using CRM.Application.Dtos.Client.Request;
using CRM.Application.Dtos.Company.Request;
using CRM.Application.Interfaces;
using CRM.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyApplication _companyApplication;
        private readonly IGenerateExcelApplicattion _generateExcelApplicattion;

        public CompanyController(ICompanyApplication companyApplication, IGenerateExcelApplicattion generateExcelApplicattion)
        {
            _companyApplication = companyApplication;
            _generateExcelApplicattion = generateExcelApplicattion;
        }
        [HttpGet("Select")]
        public async Task<IActionResult> ListSelectCompanies()
        {
            var response = await _companyApplication.ListSelectCompany();
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterCompany([FromBody] CompanyRequestDto requestDto)
        {
            var response = await _companyApplication.RegisterCompany(requestDto);
            return Ok(response);
        }
    }
}
