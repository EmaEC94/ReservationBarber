using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Dtos.Catalog.Response
{
    public class CatalogResponseDto
    {
        public int CatalogId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? AuditCreateDate { get; set; }
        public int State { get; set; }
        public string? StateCatalog { get; set; }
    }
}
