using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Domain.Entities
{
    public class Catalog : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

    }
}
