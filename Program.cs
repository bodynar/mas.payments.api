using System;
using System.IO;
using System.Text;

using MAS.Payments.Configuration;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

using Serilog;

using SimpleInjector;

var container = new Container();

var logPath = Path.Combine("logs", DateTime.Now.ToString("dd-MM-yyyy"), "app.log");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File(logPath, encoding: Encoding.UTF8)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

try
{
    Log.Information("[INIT] Configuring services");
    ServicesConfiguration.Configure(builder.Services, builder.Configuration, container);
    Log.Information("[INIT] Services configured successfully");
}
catch (Exception e)
{
    Log.Error(e, "[INIT] Service configuration failed");
    throw;
}

var app = builder.Build();

try
{
    Log.Information("[INIT] Configuring application");
    app.Configure(container, app.Environment.IsDevelopment());
    Log.Information("[INIT] Application configured successfully");
}
catch (Exception e)
{
    Log.Error(e, "[INIT] Application configuration failed");
    throw;
}

Log.Information("[INIT] Successfull");

app.Run();
