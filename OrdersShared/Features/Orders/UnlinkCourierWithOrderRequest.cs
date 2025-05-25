using MediatR;
using OrdersShared.POCOs;

namespace OrdersShared.Features.Orders
{
    public record UnlinkCourierWithOrderRequest(string courierId, string orderId) : IRequest<UnlinkCourierWithOrderRequest.Response>
    {
        public const string RouteTemplate = "api/UnlinkCourierWithOrder/{courierId}/{orderId}";

        public string GetFullRoute() => RouteTemplate.Replace("{courierId}", courierId).Replace("{orderId}", orderId);

        public record Response(bool result);
    }
}
