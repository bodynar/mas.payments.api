namespace MAS.Payments.Configuration
{
    using MAS.Payments.DataBase;
    using MAS.Payments.Infrastructure.MailMessaging;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using SimpleInjector;

    public static class ServicesConfiguration
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration, Container container)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services
                .AddMvc(options => { options.EnableEndpointRouting = false; })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson()
                .Services
                .AddDbContext<DataBaseContext>(options =>
                    options.UseSqlServer(connectionString,
                        x => x
                            .MigrationsAssembly("MAS.Payments"))
                            .UseLazyLoadingProxies()
                            .UseSqlServer(connectionString)
                )
                .AddSimpleInjector(container, options => {
                    options.AddAspNetCore()
                        .AddControllerActivation();
                })
                .AddOptions()
                .Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"))
                .AddSpaStaticFiles(cfg => {
                    cfg.RootPath = "ClientApp/dist";
                });
        }
    }
}