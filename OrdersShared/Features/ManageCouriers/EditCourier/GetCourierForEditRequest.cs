using MediatR;

namespace OrdersShared.Features.ManageCouriers.EditCourier
{
    public record GetCourierForEditRequest(int id) : IRequest<GetCourierForEditRequest.Response>
    {
        public const string RouteTemplate = "/api/couriers/{id}";

        public record Response(Courier courier);
        public record Courier(int Id, string FirstName, string MiddleName, string LastName, string? Email, string MobilePhone, string? Image);
    }
}
