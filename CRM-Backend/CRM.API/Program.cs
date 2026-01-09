using CRM.API.Extensions;
using CRM.Application.Extensions;
using CRM.Application.Services;
using CRM.Infrastructure.Extensions;
using CRM.Infrastructure.Persistences.Context;
using CRM.Utilities.AppSettings;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WatchDog;


var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;
var Cors = "Cors";



builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("Tenant_crm")));

builder.Services.AddHangfireServer();

builder.Services.AddInjectionInfreaestructure(Configuration);
builder.Services.AddInjectionApplication(Configuration);
builder.Services.AddAuthentication(Configuration);

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("GoogleSettings"));



builder.Services.AddHttpContextAccessor();//Propociona al contexto de la aplicacion

builder.Services.AddCors(options =>
{

    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("https://destinybarbercr.com", "http://localhost:4200", "http://localhost:4201", "https://localhost:7214");
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Configure(builder.Configuration.GetSection("Kestrel"));
    options.ConfigureHttpsDefaults(httpsOptions =>
    {
        // Carga tu PFX explícitamente
        httpsOptions.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(
            @"C:\certs\localhost.pfx",
            "MiPassword123"
        );

        Console.WriteLine($"Certificado cargado: {httpsOptions.ServerCertificate?.Thumbprint}");
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CrmContext>(); // Asegúrate de que `ApplicationDbContext` esté registrado
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al aplicar las migraciones a la base de datos.");
    }
}

// Dashboard de Hangfire
app.UseHangfireDashboard();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// CORS debe ir aquí, dentro del pipeline de routing y antes de auth
app.UseCors("AllowFrontend");

//Autenticación primero
app.UseAuthentication();

//Luego autorización
app.UseAuthorization();

//Luego los controladores
app.MapControllers();



/*using (var scope = app.Services.CreateScope())
{
    var jobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    jobManager.AddOrUpdate<NotificationApplication>(
        "SendNotificationActivePause",
        service => service.SendNotificationActivePause(),
        Cron.Minutely);
}
*/
app.MapHangfireDashboard();

app.Run();
