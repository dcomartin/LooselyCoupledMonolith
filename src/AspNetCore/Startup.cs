using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
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
            services.AddControllers();

            //services.AddHostedService<NServiceBusHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapSales();
                endpoints.MapShipping();
            });
        }
    }
}

public class NServiceBusHostedService : BackgroundService
{
    private IEndpointInstance _endpointInstance;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var endpointConfiguration = new EndpointConfiguration("Demo");

        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();

        _endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _endpointInstance.Stop();
        await base.StopAsync(cancellationToken);
    }
}
