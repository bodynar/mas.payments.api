namespace MAS.Payments.Configuration
{
    using MAS.Payments.DataBase;

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
                .AddNewtonsoftJson()
            .Services
                .AddDbContext<DataBaseContext>(x => x
                    .UseNpgsql(connectionString)
                    .UseLazyLoadingProxies()
                )
                .AddSimpleInjector(container, options => {
                    options.AddAspNetCore()
                        .AddControllerActivation();
                })
                .AddOptions()
                .AddSpaStaticFiles(cfg => {
                    cfg.RootPath = "ClientApp/dist";
                });
        }
    }
}