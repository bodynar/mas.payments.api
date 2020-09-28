namespace MAS.Payments
{
    using System.Text;

    using MAS.Payments.Configuration;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Serilog;

    using SimpleInjector;

    public class Startup
    {
        private Container Container { get; }

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Container = new Container();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs\\myapp.txt", rollingInterval: RollingInterval.Day, encoding: Encoding.UTF8)
                .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                Log.Information($"Calling {nameof(ConfigureServices)}");
                ServicesConfiguration.Configure(services, Configuration, Container);
            }
            catch (System.Exception e)
            {
                Log.Error(e, $"Exception in {nameof(ConfigureServices)}");
                throw e;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
            {
                Log.Information($"Calling {nameof(Configure)}");
                app.Configure(Container, env.IsDevelopment());
            }
            catch (System.Exception e)
            {
                Log.Error(e, $"Exception in {nameof(Configure)}");
                throw e;
            }
        }
    }
}