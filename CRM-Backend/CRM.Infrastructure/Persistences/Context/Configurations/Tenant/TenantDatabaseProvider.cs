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
            // Obtener la cadena de conexión del tenant
            // Esto puede ser desde un archivo de configuración, una base de datos maestra, o un servicio externo
            //"Data Source=DESKTOP-U2JP9CC\\SQLEXPRESS;Database=POS-SOARSA;User Id=sa;Password=Soa-15111989;TrustServerCertificate=True;"

            return _configuration.GetConnectionString($"Tenant_{tenant}")!;
        }
    }
}
