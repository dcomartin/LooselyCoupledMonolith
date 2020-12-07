using Billing.Contracts;
using NServiceBus;

namespace Billing
{
    public static class Endpoints
    {
        public static void MapBilling(this RoutingSettings<LearningTransport> routing)
        {
            routing.RouteToEndpoint(
                assembly: typeof(OrderBilled).Assembly,
                destination: "LooselyCoupledMonolith");
        }
    }
}