using System;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Sales.Contracts;

namespace Sales
{
    public static class Endpoints
    {
        public static void MapSales(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/sales", async context =>
            {
                var orderPlaced = new OrderPlaced
                {
                    OrderId = Guid.NewGuid()
                };
                await context.RequestServices.GetService<ICapPublisher>().PublishAsync(nameof(OrderPlaced), orderPlaced);

                await context.Response.WriteAsync($"Order {orderPlaced.OrderId} has been placed.");
            });
        }
    }
}