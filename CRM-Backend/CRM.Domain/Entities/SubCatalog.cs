using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CRM.Domain.Entities
{
    public class SubCatalog : BaseEntity
    {
        public int CatalogId { get; set; } 
        public virtual Catalog Catalog { get; set; } = null!;
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int? Price { get; set; }
        public string? Description { get; set; }
        public TimeOnly? Duration { get; set; }

    }
}
