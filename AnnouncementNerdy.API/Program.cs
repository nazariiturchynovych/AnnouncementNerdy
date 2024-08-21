using System.Text.Json;
using AnnouncementNerdy.Application;
using AnnouncementNerdy.Infrastructure;
using Serilog;
using Serilog.Events;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{level:u3}] {Message:lj}{NewLine}{Exception}",
        restrictedToMinimumLevel: LogEventLevel.Information)
    .CreateBootstrapLogger();


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (hb, lc) => lc
        .WriteTo.Console()
        .MinimumLevel.Information()
        .ReadFrom.Configuration(hb.Configuration));

Log.Fatal(JsonSerializer.Serialize(builder.Configuration));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .RegisterApplication()
    .RegisterInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();