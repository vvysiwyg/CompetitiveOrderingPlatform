using MediatR;
using OrdersShared.POCOs;

namespace OrdersShared.Features.Couriers
{
    public record GetOIPSRequest : IRequest<GetOIPSRequest.Response>
    {
        public const string RouteTemplate = "api/GetOIPS";

        public record Response(List<OrderIssuePoint>? oips);
    }
}
