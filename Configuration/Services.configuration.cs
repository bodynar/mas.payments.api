using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure.MailMessaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace MAS.Payments.Configuration
{
    public static class ServicesConfiguration
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration, Container container)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(JsonConfiguration.Configure);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(cfg =>
            {
                cfg.RootPath = "ClientApp/dist";
            });

            services.AddSimpleInjector(container, options =>
            {
                options.AddAspNetCore()
                    .AddControllerActivation();
            });

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<DataBaseContext>(options =>
                options.UseSqlServer(connectionString, 
                    x => x
                        .MigrationsAssembly("MAS.Payments"))
                        .UseLazyLoadingProxies()
                        .UseSqlServer(connectionString)
            );

            #region Mail
            
            services.AddOptions();
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));

            #endregion
        }
    }
}