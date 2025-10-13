using Microsoft.AspNetCore.Http;

namespace CRM.Infrastructure.Persistences.Context.Configurations.Tenant.Resolver
{
    public class TenantResolver : ITenantResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string ResolveTenant()
        {
            var tenant = "crm"; 
                //_httpContextAccessor.HttpContext?.Request.Headers["X-Tenant-ID"].FirstOrDefault();
            if (string.IsNullOrEmpty(tenant))
            {
                throw new InvalidOperationException("No se propocione Tenant");
            }
            return tenant;
        }
    }
}
