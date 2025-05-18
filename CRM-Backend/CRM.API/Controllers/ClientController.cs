using CRM.Application.Commons.Bases.Request;
using CRM.Application.Dtos.Client.Request;
using CRM.Application.Interfaces;
using CRM.Utilities.Static;
using Microsoft.AspNetCore.Mvc;


namespace CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientApplication _clientApplication;
        private readonly IGenerateExcelApplicattion _generateExcelApplicattion;

        public ClientController(IGenerateExcelApplicattion generateExcelApplicattion, IClientApplication clientApplication)
        {
            _generateExcelApplicattion = generateExcelApplicattion;
            _clientApplication = clientApplication;
        }

        [HttpGet]
        public async Task<IActionResult> ListClients([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _clientApplication.ListClient(filters);
            if (filters.Download.HasValue && filters.Download.Value)
            {
                var columnNames = ExcelColumnNames.GetColumnsClients();
                var fileBytes = _generateExcelApplicattion.GenerateToExcel(response.Data!, columnNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }
            return Ok(response);
        }
        [HttpGet("Select")]
        public async Task<IActionResult> ListSelectClients()
        {
            var response = await _clientApplication.ListSelectClient();
            return Ok(response);
        }
        [HttpGet("{clientId:int}")]
        public async Task<IActionResult> ClientById(int clientId)
        {
            var response = await _clientApplication.ClientById(clientId);
            return Ok(response);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterClient([FromBody] ClientRequestDto requestDto)
        {
            var response = await _clientApplication.RegisterClient(requestDto);
            return Ok(response);
        }

        [HttpPut("Edit/{clientId}")]
        public async Task<IActionResult> EditClient(int clientId, [FromBody] ClientRequestDto requestDto)
        {
            var response = await _clientApplication.EditClient(clientId, requestDto);
            return Ok(response);
        }
       
        [HttpPut("Remove/{clientId:int}")]
        public async Task<IActionResult> RemoveClient(int clientId)
        {
            var response = await _clientApplication.RemoveClient(clientId);
            return Ok(response);
        }
    }
}
