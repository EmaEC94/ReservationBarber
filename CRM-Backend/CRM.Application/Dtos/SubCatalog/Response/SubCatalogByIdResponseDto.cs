using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Dtos.SubCatalog.Response
{
    public class SubCatalogByIdResponseDto
    {
        public int SubCatalogId { get; set; }
        public int CatalogId { get; set; }
        public string? Name { get; set; }
        public string? code { get; set; }
        public int? price { get; set; }
        public string? Description { get; set; }
        public TimeOnly Duration { get; set; }
    }
}
