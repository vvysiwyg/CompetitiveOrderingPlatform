using MediatR;
namespace OrdersShared.Features.Orders
{
    public record LinkCourierWithOrderRequest(int courierId, int orderId, double deliveryCost) : IRequest<LinkCourierWithOrderRequest.Response>
    {
        public const string RouteTemplate = "api/LinkCourierWithOrder";

        public record Response(bool result);
    }
}
