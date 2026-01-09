using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Dtos.Catalog.Response
{
    public class CatalogByIdResponseDto
    {
        public int CatalogId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
