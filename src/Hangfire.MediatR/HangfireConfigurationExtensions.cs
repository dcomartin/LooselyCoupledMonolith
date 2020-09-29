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
            var jsonSerializerSettings =  new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
            };
            GlobalConfiguration.Configuration.UseSerializerSettings(jsonSerializerSettings);
        }
    }
}