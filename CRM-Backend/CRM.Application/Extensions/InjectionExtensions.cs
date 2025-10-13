using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using CRM.Application.Commons.Ordering;
using CRM.Application.Extensions.WhatchDog;
using CRM.Application.Interfaces;
using CRM.Application.Services;
using System.Reflection;


namespace CRM.Application.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);



            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<IOrderingQuery, OrderingQuery>();
            services.AddTransient<IFileStoreLocalApplication, FileStoreLocalApplication>();
            services.AddScoped<IGenerateExcelApplicattion, GenerateExcelApplication>();
            services.AddScoped<IClientApplication, ClientApplication>();
            services.AddScoped<IDocumentTypeApplication, DocumentTypeApplication>();
            services.AddScoped<ICompanyApplication, CompanyApplication>();
            services.AddScoped<IActivePauseApplication, ActivePauseApplication>();
            services.AddScoped<INotificationApplication, NotificationApplication>();
            services.AddScoped<IWhatsappCloudApplication, WhatsappCloudApplication>();
            services.AddScoped<IEmailApplication, EmailApplication>();
            services.AddScoped<IUserApplication, UserApplication>();
            services.AddScoped<IAuthApplication, AuthApplication>();
            services.AddScoped<IReservationApplication, ReservationApplication>();



            //services.AddWhatchDog(configuration);

            return services;


        }
    }
}
