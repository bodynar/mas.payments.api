using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using SimpleInjector;

namespace MAS.Payments.Configuration
{
    public static class AppConfiguration
    {
        public static void Configure(this IApplicationBuilder app, Container container, bool isDevelopment)
        {
            if (isDevelopment)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app
                .UseMiddleware<ExceptionHandlerMiddleware>()
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseSimpleInjector(container, options => { })
                .UseMvc(routes => {
                    routes.MapRoute(
                        name: "default",
                        template: "api/{controller}/{action}/{id?}");
                })
                .UseSpaStaticFiles();

            app.UseSpa(spa => {
                spa.Options.SourcePath = "ClientApp";

                if (isDevelopment)
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            container
                .Configure()
                .Verify();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DataBaseContext>();
                context.Database.EnsureCreated();
            }
        }
    }
}