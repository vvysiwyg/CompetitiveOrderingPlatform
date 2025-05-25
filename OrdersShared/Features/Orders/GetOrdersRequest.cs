using MediatR;
using OrdersShared.POCOs;

namespace OrdersShared.Features.Orders
{
    public record GetOrdersRequest(string status) : IRequest<GetOrdersRequest.Response>
    {
        public const string RouteTemplate = "api/GetOrders/{status}";

        public string GetFullRoute() => RouteTemplate.Replace("{status}", status);

        public record Response(List<Order>? orders);
    }
}
