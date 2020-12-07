using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NServiceBus;
using Shipping.Contracts;

namespace Shipping
{
    public static class Endpoints
    {
        public static void MapShipping(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/shipping", async context =>
            {
                await context.Response.WriteAsync("Hello Shipping!");
            });
        }

        public static void MapShipping(this RoutingSettings<LearningTransport> routing)
        {
            routing.RouteToEndpoint(
                assembly: typeof(ShippingLabelCreated).Assembly,
                destination: "LooselyCoupledMonolith");
        }
    }
}