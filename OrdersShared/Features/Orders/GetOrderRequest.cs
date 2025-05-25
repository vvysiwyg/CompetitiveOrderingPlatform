using MediatR;
using OrdersShared.POCOs;

namespace OrdersShared.Features.Orders
{
    public record GetOrderRequest(int id) : IRequest<GetOrderRequest.Response>
    {
        public const string RouteTemplate = "api/GetOrder/{id:int}";

        public string GetFullRoute() => RouteTemplate.Replace("{id:int}", id.ToString());

        public record Response(Order? order);
    }
}
