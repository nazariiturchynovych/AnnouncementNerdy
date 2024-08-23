using AnnouncementNerdy.Application;
using AnnouncementNerdy.Infrastructure;
using AnnouncementNerdy.Middlewares;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Events;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{level:u3}] {Message:lj}{NewLine}{Exception}",
        restrictedToMinimumLevel: LogEventLevel.Information)
    .CreateBootstrapLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog(
        (hb, lc) => lc
            .WriteTo.Console()
            .MinimumLevel.Information()
            .ReadFrom.Configuration(hb.Configuration));
    
    builder.Services.AddControllers();
    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    builder.Services.AddHealthChecks()
        .AddElasticsearch(builder.Configuration.GetSection("ElasticsearchSettings:uri").Value,"Elastic");
    
    builder.Services
        .RegisterApplication()
        .RegisterInfrastructure(builder.Configuration);

    builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
    
    //-------------------------------------------------//
    
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapHealthChecks("/_health", new HealthCheckOptions() {ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse});
    
    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

    app.UseHttpsRedirection();
    
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex.Message, "Exception in program.cs occured");
}
finally
{
    Log.Information("shut down complete");
    Log.CloseAndFlush();
}

