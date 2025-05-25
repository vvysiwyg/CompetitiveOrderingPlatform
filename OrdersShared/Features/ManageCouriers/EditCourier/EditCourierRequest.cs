using FluentValidation;
using MediatR;
using OrdersShared.Features.ManageCouriers.Shared;

namespace OrdersShared.Features.ManageCouriers.EditCourier
{
    public record EditCourierRequest(CourierDto courier) : IRequest<EditCourierRequest.Response>
    {
        public const string RouteTemplate = "/api/couriers";

        public record Response(bool isSuccess);
    }

    public class EditCourierRequestValidator : AbstractValidator<EditCourierRequest>
    {
        public EditCourierRequestValidator()
        {
            RuleFor(x => x.courier).SetValidator(new CourierValidator());
        }
    }
}
