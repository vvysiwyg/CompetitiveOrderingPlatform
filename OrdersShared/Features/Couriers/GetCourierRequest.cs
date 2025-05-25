using MediatR;
using OrdersShared.POCOs;

namespace OrdersShared.Features.Orders
{
    public record GetCourierRequest(int id) : IRequest<GetCourierRequest.Response>
    {
        public const string RouteTemplate = "api/GetCourier/{id:int}";

        public string GetFullRoute() => RouteTemplate.Replace("{id:int}", id.ToString());

        public record Response(Courier? courier);
    }
}
