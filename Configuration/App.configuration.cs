using MAS.Payments.DataBase;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace MAS.Payments.Configuration
{
    public static class AppConfiguration
    {
        public static void Configure(this IApplicationBuilder app, Container container, IHostingEnvironment hostingEnvironment)
        {
            var isDevelopment = hostingEnvironment.IsDevelopment();

            if (isDevelopment)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller}/{action}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (isDevelopment)
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            app.UseSimpleInjector(container, options => { });

            container.Configure();
            container.Verify();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DataBaseContext>();
                context.Database.EnsureCreated();
            }
        }
    }
}