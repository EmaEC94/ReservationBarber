using CRM.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentTypeController : ControllerBase
    {
        private readonly IDocumentTypeApplication documentTypeApplication;

        public DocumentTypeController(IDocumentTypeApplication IDocumentTypeApplication)
        {
            this.documentTypeApplication = IDocumentTypeApplication;
        }

  
        [HttpGet]
        public async Task<IActionResult> ListDocumenttype()
        {
            var response = await documentTypeApplication.ListDocumentTypes();
            return Ok(response);
        }
    }
}
