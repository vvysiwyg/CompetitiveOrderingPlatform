using FluentValidation;

namespace OrdersShared.Features.ManageCouriers.Shared
{
    public class CourierDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string MiddleName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string MobilePhone { get; set; } = string.Empty;

        public string? Image { get; set; }

        public ImageAction ImageAction { get; set; }
    }

    public enum ImageAction
    {
        None,
        Add,
        Remove
    }

    public class CourierValidator : AbstractValidator<CourierDto>
    {
        public CourierValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Укажите имя");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Укажите фамилию");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Укажите email в правильном формате");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Укажите email");
            RuleFor(x => x.MobilePhone).NotEmpty().WithMessage("Укажите номер телефона");
        }
    }
}
