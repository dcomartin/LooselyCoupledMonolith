using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;
using Sales.Contracts;

namespace Shipping
{
    public static class ConfigureServices
    {
        public static void AddShipping(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<CreateShippingLabel>();
        }

        public static void AddShipping(this IBrighterHandlerBuilder brighterHandlerBuilder)
        {
            brighterHandlerBuilder.AsyncHandlersFromAssemblies(typeof(ConfigureServices).Assembly);
            brighterHandlerBuilder.HandlersFromAssemblies(typeof(ConfigureServices).Assembly);
            brighterHandlerBuilder.MapperRegistry(registry =>
            {
                registry.Register<OrderPlaced, OrderPlacedMapper>();
            });
        }
    }

    public class OrderPlacedMapper : IAmAMessageMapper<OrderPlaced>
    {
        public Message MapToMessage(OrderPlaced request)
        {
            var header = new MessageHeader(messageId: request.Id, topic: "demo", messageType: MessageType.MT_EVENT);
            var body = new MessageBody(JsonConvert.SerializeObject(request));
            var message = new Message(header, body);
            return message;
        }

        public OrderPlaced MapToRequest(Message message)
        {
            return JsonConvert.DeserializeObject<OrderPlaced>(message.Body.Value);
        }
    }
}
