using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;

namespace Sales
{
    public static class ConfigureServices
    {
        public static void AddSales(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<SalesDbContext>(builder =>
            {
                builder.UseInMemoryDatabase("Sales");
            });

            serviceCollection.AddMediatR(typeof(ConfigureServices).Assembly);
        }

        public static IBrighterHandlerBuilder AddSales(this IBrighterHandlerBuilder brighterHandlerBuilder)
        {
            brighterHandlerBuilder.AsyncHandlersFromAssemblies(typeof(ConfigureServices).Assembly);
            brighterHandlerBuilder.HandlersFromAssemblies(typeof(ConfigureServices).Assembly);
            brighterHandlerBuilder.MapperRegistry(registry =>
            {
                registry.Register<PlaceOrder, PlaceOrderMapper>();
            });
            return brighterHandlerBuilder;
        }

        public class PlaceOrderMapper : IAmAMessageMapper<PlaceOrder>
        {
            public Message MapToMessage(PlaceOrder request)
            {
                var header = new MessageHeader(messageId: request.Id, topic: "demo", messageType: MessageType.MT_COMMAND);
                var body = new MessageBody(JsonConvert.SerializeObject(request));
                var message = new Message(header, body);
                return message;
            }

            public PlaceOrder MapToRequest(Message message)
            {
                return JsonConvert.DeserializeObject<PlaceOrder>(message.Body.Value);
            }
        }
    }
}
