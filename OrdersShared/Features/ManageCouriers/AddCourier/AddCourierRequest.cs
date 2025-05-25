using FluentValidation;
using MediatR;
using OrdersShared.Features.ManageCouriers.Shared;

namespace OrdersShared.Features.ManageCouriers.AddCourier
{
    public record AddCourierRequest(CourierDto courier) : IRequest<AddCourierRequest.Response>
    {
        public const string RouteTemplate = "add-courier";

        public record Response(int id);
    }

    public class AddCourierRequestValidator : AbstractValidator<AddCourierRequest>
    {
        public AddCourierRequestValidator()
        {
            RuleFor(x => x.courier).SetValidator(new CourierValidator());
        }
    }
}
