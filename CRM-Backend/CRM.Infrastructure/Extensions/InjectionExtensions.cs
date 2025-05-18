using CRM.Infrastructure.FileExcel;
using CRM.Infrastructure.FileStorage;
using CRM.Infrastructure.Persistences.Context;
using CRM.Infrastructure.Persistences.Context.Configurations.Tenant.Resolver;
using CRM.Infrastructure.Persistences.Context.Configurations.Tenant;
using CRM.Infrastructure.Persistences.Interfaces;
using CRM.Infrastructure.Persistences.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.Infrastructure.Extensions
{
    public  static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionInfreaestructure(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(CrmContext).Assembly.FullName;

            //services.AddDbContext<CrmContext>(
            //    options=>options.UseSqlServer(
            //        configuration.GetConnectionString("CRMConnection"),b=> b.MigrationsAssembly(assembly)), ServiceLifetime.Transient );

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IGenerateExcel, GenerateExcel>();
            services.AddTransient<IFileStorageLocal, FileStorageLocal>();
            services.AddTransient<IAzureStorage, AzureStorage>();         
            services.AddScoped<IEmailRepository, EmailRepository>();

            //Tenant
            services.AddScoped<ITenantResolver, TenantResolver>();
            services.AddScoped<TenantDatabaseProvider>();

            services.AddScoped(provider =>
            {
                var tenantProvider = provider.GetRequiredService<TenantDatabaseProvider>();
                var connectionString = tenantProvider.GetConnectionString();
                return new CrmContext(connectionString);

            });

            return services;
        }
    }
}
