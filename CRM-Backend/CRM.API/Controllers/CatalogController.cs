using CRM.Application.Commons.Bases.Request;
using CRM.Application.Dtos.Catalog.Request;
using CRM.Application.Interfaces;
using CRM.Utilities.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController  : ControllerBase
    {
        private readonly ICatalogApplication _catalogApplication;
        private readonly IGenerateExcelApplicattion _generateExcelApplicattion;
    public CatalogController(ICatalogApplication catalogApplication, IGenerateExcelApplicattion generateExcelApplicattion)
    {
         _catalogApplication = catalogApplication;
        _generateExcelApplicattion = generateExcelApplicattion;
    }
    [HttpGet]
    public async Task<IActionResult> ListCatalog([FromQuery] BaseFiltersRequest filters)
    {
        var response = await _catalogApplication.ListCatalog(filters);
        if (filters.Download.HasValue && filters.Download.Value)
        {
            var columnNames = ExcelColumnNames.GetColumnsUsers();
            var fileBytes = _generateExcelApplicattion.GenerateToExcel(response.Data!, columnNames);
            return File(fileBytes, ContentType.ContentTypeExcel);
        }
        return Ok(response);
    }
    [HttpGet("Select")]
    public async Task<IActionResult> ListSelectCatalog()
    {
        var response = await _catalogApplication.ListSelectCatalog();
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterCatalog([FromForm] CatalogRequestDto requestDto)
    {
        var response = await _catalogApplication.RegisterCatalog(requestDto);
        return Ok(response);
    }

    [HttpGet("{catalogId:int}")]
    public async Task<IActionResult> CatalogById(int catalogId)
    {
        var response = await _catalogApplication.CatalogById(catalogId);
        return Ok(response);
    }
    [HttpPut("Edit/{catalogId}")]
    public async Task<IActionResult> EditCatalog(int catalogId, [FromForm] CatalogRequestDto requestDto)
    {
        var response = await _catalogApplication.EditCatalog(catalogId, requestDto);
        return Ok(response);
    }
    [HttpPut("Remove/{catalogId:int}")]
    public async Task<IActionResult> RemoveCatalog(int catalogId)
    {
        var response = await _catalogApplication.RemoveCatalog(catalogId);
        return Ok(response);
    }
}
}
