using System;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;
using Paramore.Brighter.MessagingGateway.RMQ;
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
            services.AddControllers();

            services.AddSales();
            services.AddShipping();

            services
                .AddLogging(builder =>
                {
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddConsole();
                })
                .AddBrighter(options =>
                {
                    var messageStore = new InMemoryMessageStore();
                    var rmq = new RmqMessageProducer(new RmqMessagingGatewayConnection
                    {
                        AmpqUri = new AmqpUriSpecification(new Uri("amqp://guest:guest@localhost:5672")),
                        Exchange = new Exchange("demo")
                    });
                    options.BrighterMessaging = new BrighterMessaging(messageStore, rmq);
                })
                .AddSales()
                .AddShipping();;
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
                endpoints.MapSales();
                endpoints.MapShipping();
                endpoints.MapControllers();
            });
        }
    }
}
