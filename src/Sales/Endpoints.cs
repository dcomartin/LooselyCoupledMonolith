using Microsoft.AspNetCore.Routing;
using NServiceBus;
using Sales.Contracts;

namespace Sales
{
    public static class Endpoints
    {
        public static void MapSales(this IEndpointRouteBuilder endpoints)
        {

        }

        public static void MapSales(this RoutingSettings<LearningTransport> routing)
        {
            routing.RouteToEndpoint(
                assembly: typeof(OrderPlaced).Assembly,
                destination: "LooselyCoupledMonolith");
        }
    }
}