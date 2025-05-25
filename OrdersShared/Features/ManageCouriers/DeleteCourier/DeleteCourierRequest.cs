using MediatR;
using OrdersShared.Features.ManageCouriers.Shared;

namespace OrdersShared.Features.ManageCouriers.DeleteCourier
{
    public record DeleteCourierRequest(List<int> ids) : IRequest<DeleteCourierRequest.Response>
    {
        public const string RouteTemplate = "/api/courier";

        public record Response(bool areAllDeleted);
    }
}
