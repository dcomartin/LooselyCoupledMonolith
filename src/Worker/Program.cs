using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;
using Paramore.Brighter.MessagingGateway.RMQ;
using Paramore.Brighter.ServiceActivator;
using Sales;
using Shipping;

namespace Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSales();
                    services.AddShipping();

                    services
                        .AddBrighter(options =>
                        {
                            var messageStore = new InMemoryMessageStore();
                            var rmq = new RmqMessageProducer(new RmqMessagingGatewayConnection
                            {
                                AmpqUri = new AmqpUriSpecification(new Uri("amqp://guest:guest@localhost:5672/%2f")),
                                Exchange = new Exchange("demo")
                            });
                            options.BrighterMessaging = new BrighterMessaging(messageStore, rmq);
                        })
                        .AddSales()
                        .AddShipping();

                    services.AddHostedService<BrighterHostedService>();
                });

    }

    public class BrighterHostedService : BackgroundService
    {
        private readonly IDispatcher _dispatcher;

        private static MessageMapperRegistry MessageMapperRegistry(IServiceProvider provider)
        {
            var serviceCollectionMessageMapperRegistry = provider.GetService<ServiceCollectionMessageMapperRegistry>();

            var messageMapperRegistry = new MessageMapperRegistry(new ServiceProviderMapperFactory(provider));

            foreach (var messageMapper in serviceCollectionMessageMapperRegistry)
            {
                messageMapperRegistry.Add(messageMapper.Key, messageMapper.Value);
            }

            return messageMapperRegistry;
        }

        public BrighterHostedService(IServiceProvider provider, ServiceCollectionSubscriberRegistry subscriberRegistry, IBrighterOptions brighterOptions)
        {
            var handlerFactory = new ServiceProviderHandlerFactory(provider);
            var messageMapperRegistry = MessageMapperRegistry(provider);

            var rmqConnnection = new RmqMessagingGatewayConnection
            {
                AmpqUri = new AmqpUriSpecification(new Uri("amqp://guest:guest@localhost:5672/%2f")),
                Exchange = new Exchange("demo"),
            };
            var rmqMessageConsumerFactory = new RmqMessageConsumerFactory(rmqConnnection);

            _dispatcher = DispatchBuilder.With()
                .CommandProcessor(CommandProcessorBuilder.With()
                    .Handlers(new HandlerConfiguration(subscriberRegistry, (IAmAHandlerFactory) handlerFactory))
                    .Policies(brighterOptions.PolicyRegistry)
                    .NoTaskQueues()
                    .RequestContextFactory(new InMemoryRequestContextFactory())
                    .Build())
                .MessageMappers(messageMapperRegistry)
                .DefaultChannelFactory(new ChannelFactory(rmqMessageConsumerFactory))
                .Connections(new []
                {
                    new Connection<PlaceOrder>(
                        new ConnectionName("demo"),
                        new ChannelName("demo"),
                        new RoutingKey("demo"),
                        timeoutInMilliseconds : 200)
                }).Build();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _dispatcher.Receive();
            return Task.CompletedTask;
        }
    }
}
