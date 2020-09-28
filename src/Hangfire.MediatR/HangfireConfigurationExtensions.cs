using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Hangfire.MediatR
{
    public static class HangfireConfigurationExtensions
    {
        public static void AddHangfireMessaging(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<MediatorHangfireBridge>();
            serviceCollection.AddHostedService<HangfireMediatorHostedService>();
        }
    }

    public class HangfireMediatorHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public HangfireMediatorHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            GlobalConfiguration.Configuration.UseActivator(new MediatorJobActivator(_serviceProvider));
            var jsonSerializerSettings =  new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
            };
            GlobalConfiguration.Configuration.UseSerializerSettings(jsonSerializerSettings);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}