using MediatR;
using OrdersShared.POCOs;

namespace OrdersShared.Features.Orders
{
    public record GetCouriersRequest() : IRequest<GetCouriersRequest.Response>
    {
        public const string RouteTemplate = "api/GetCouriers";

        public record Response(List<Courier>? couriers);
    }
}
