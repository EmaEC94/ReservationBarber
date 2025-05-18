using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WatchDog;
using WatchDog.src.Enums;


namespace CRM.Application.Extensions.WhatchDog
{
    public static class  WathcDogExtensions
    {
        public static IServiceCollection AddWhatchDog(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWatchDogServices(options =>
            {
                options.SetExternalDbConnString = configuration.GetConnectionString("CRMConnectionServer");
                options.SqlDriverOption = WatchDogSqlDriverEnum.MSSQL;
                options.IsAutoClear = true;
                options.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Daily;
            });
            return services;
        }
    }
}
