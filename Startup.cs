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
                Log.Debug($"Trying to \"{nameof(ConfigureServices)}\"");
                ServicesConfiguration.Configure(services, Configuration, Container);
                Log.Debug($"\"{nameof(ConfigureServices)}\": SUCCESS");
            }
            catch (System.Exception e)
            {
                Log.Error(e, $"\"{nameof(ConfigureServices)}\": FAILED");
                throw;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
            {
                Log.Debug($"Trying to \"{nameof(Configure)}\"");
                app.Configure(Container, env.IsDevelopment());
                Log.Debug($"\"{nameof(Configure)}\": SUCCESS");
            }
            catch (System.Exception e)
            {
                Log.Error(e, $"\"{nameof(Configure)}\": FAILED");
                throw;
            }
        }
    }
}