using DotNetCore.CAP;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sales;
using Shipping;

namespace AspNetCore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSales();
            services.AddShipping();

            services.AddCap(options =>
            {
                options.ConsumerThreadCount = 0;
                options.UseInMemoryStorage();
                options.UseRabbitMQ("localhost");
                options.UseDashboard();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCapDashboard();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSales();
                endpoints.MapShipping();
            });
        }
    }
}
