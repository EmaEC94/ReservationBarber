using CRM.Application.Commons.Bases.Request;
using CRM.Application.Dtos.Catalog.Request;
using CRM.Application.Dtos.SubCatalog.Request;
using CRM.Application.Interfaces;
using CRM.Utilities.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCatalogController : ControllerBase
    {
        private readonly ISubCatalogApplication _subCatalogApplication;
        private readonly IGenerateExcelApplicattion _generateExcelApplicattion;
        public SubCatalogController(ISubCatalogApplication subCatalogApplication, IGenerateExcelApplicattion generateExcelApplicattion)
        {
            _subCatalogApplication = subCatalogApplication;
            _generateExcelApplicattion = generateExcelApplicattion;
        }
        [HttpGet]
        public async Task<IActionResult> ListSubCatalog([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _subCatalogApplication.ListSubCatalog(filters);
            if (filters.Download.HasValue && filters.Download.Value)
            {
                var columnNames = ExcelColumnNames.GetColumnsUsers();
                var fileBytes = _generateExcelApplicattion.GenerateToExcel(response.Data!, columnNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }
            return Ok(response);
        }
        [HttpGet("Select")]
        public async Task<IActionResult> ListSelectSubCatalog()
        {
            var response = await _subCatalogApplication.ListSelectSubCatalog();
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterSubCatalog([FromForm] SubCatalogRequestDto requestDto)
        {
            var response = await _subCatalogApplication.RegisterSubCatalog(requestDto);
            return Ok(response);
        }

        [HttpGet("{subCatalogId:int}")]
        public async Task<IActionResult> SubCatalogById(int subCatalogId)
        {
            var response = await _subCatalogApplication.SubCatalogById(subCatalogId);
            return Ok(response);
        }
        [HttpPut("Edit/{subCatalogId}")]
        public async Task<IActionResult> EditSubCatalog(int subCatalogId, [FromForm] SubCatalogRequestDto requestDto)
        {
            var response = await _subCatalogApplication.EditSubCatalog(subCatalogId, requestDto);
            return Ok(response);
        }
        [HttpPut("Remove/{subCatalogId:int}")]
        public async Task<IActionResult> RemoveSubCatalog(int subCatalogId)
        {
            var response = await _subCatalogApplication.RemoveSubCatalog(subCatalogId);
            return Ok(response);
        }
    }
}
