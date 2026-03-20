using System;
using System.Text;

using MAS.Payments.Configuration;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

using Serilog;

using SimpleInjector;

var container = new Container();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day, encoding: Encoding.UTF8)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

try
{
    Log.Debug("Configuring services");
    ServicesConfiguration.Configure(builder.Services, builder.Configuration, container);
    Log.Debug("Services configured successfully");
}
catch (Exception e)
{
    Log.Error(e, "Service configuration failed");
    throw;
}

var app = builder.Build();

try
{
    Log.Debug("Configuring application");
    app.Configure(container, app.Environment.IsDevelopment());
    Log.Debug("Application configured successfully");
}
catch (Exception e)
{
    Log.Error(e, "Application configuration failed");
    throw;
}

app.Run();
