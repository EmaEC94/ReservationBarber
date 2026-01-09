using CRM.Infrastructure.Persistences.Context.Configurations.Tenant.Resolver;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Persistences.Context.Configurations.Tenant
{
    public class TenantDatabaseProvider
    {
        private readonly ITenantResolver _tenantResolver;
        private readonly IConfiguration _configuration;

        public TenantDatabaseProvider(ITenantResolver tenantResolver, IConfiguration configuration)
        {
            _tenantResolver = tenantResolver;
            _configuration = configuration;
        }

        public string GetConnectionString()
        {
            var tenant = _tenantResolver.ResolveTenant();
            var conn = _configuration.GetConnectionString($"Tenant_{tenant}");
            if (string.IsNullOrWhiteSpace(conn))
                conn = _configuration.GetConnectionString("DefaultConnection");
            return conn!;
        }

    }
}
