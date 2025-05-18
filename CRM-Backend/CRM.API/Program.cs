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
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("CRMConnection")));

builder.Services.AddHangfireServer();

builder.Services.AddInjectionInfreaestructure(Configuration);
builder.Services.AddIjectionApplication(Configuration);
builder.Services.AddAuthentication(Configuration);

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("GoogleSettings"));



builder.Services.AddHttpContextAccessor();//Propociona al contexto de la aplicacion

builder.Services.AddCors(options =>
{


    options.AddPolicy(name: Cors,
        builder =>
        {
            builder.WithOrigins("http://localhost:4200", "http://localhost:4201", "https://localhost:7214");
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();

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



app.UseHangfireDashboard();
app.UseCors(Cors);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseWatchDogExceptionLogger();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

/*app.UseWatchDog(configuration =>
{
    configuration.WatchPageUsername = Configuration.GetSection("WatchDog:UserName").Value;
    configuration.WatchPagePassword = Configuration.GetSection("WatchDog:Password").Value;
});*/

using (var scope = app.Services.CreateScope())
{
    var jobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    jobManager.AddOrUpdate<NotificationApplication>(
        "SendNotificationActivePause",
        service => service.SendNotificationActivePause(),
        Cron.Minutely);
}

app.MapHangfireDashboard();

app.Run();
