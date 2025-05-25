namespace OrdersWebApp.Features.Couriers
{
    public record LinkCourierWithOrderArgs(bool isOpen, string selectedCourierId, string selectedCourierFullName);
}
