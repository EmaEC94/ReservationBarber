using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CRM.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            var openApi = new OpenApiInfo
            {
                Title = "CRM API",
                Version = "v1",
                Description = "CRM SOFT WOLF 2025",
                TermsOfService = new Uri("https://opensource.org/licenses/NIT"),
                Contact = new OpenApiContact
                {
                    Name = "SoftWolf",
                    Email = "emanuel.esquivel94@gmail.com",
                    Url = new Uri("https://solfwolfcr.com")
                },
                License = new OpenApiLicense
                {
                    Name = "User under LICK",
                    Url = new Uri("https://opensource.org/licenses/")
                }

            };
            services.AddSwaggerGen(x =>
            {
                openApi.Version = "v1";
                x.SwaggerDoc("v1", openApi);

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = " Jwt Authebtication",
                    Description = "Jwt Bearer Token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                x.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[]{} }
                });
            });
            return services;
        }
    }
}
