using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Persistences.Context.Configurations.Tenant.Resolver
{
    public interface ITenantResolver
    {
        string ResolveTenant();
    }
}
