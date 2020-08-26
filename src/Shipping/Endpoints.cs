using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

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
    }
}