using OrdersShared.POCOs;

namespace OrdersWebApp.Features.Orders
{
    public static class Helper
    {
        public static string GetFullName(Customer? customer)
            => $"{customer?.LastName} {customer?.FirstName} {customer?.MiddleName}";
        public static string GetFormattedPhoneNumber(string? phoneNum)
            => phoneNum ?? "";
        public static string GetFullAddress(Order? order)
            => $"г. {order?.Delivery?.Address?.City}, ул. {order?.Delivery?.Address?.StreetName}, д. {order?.Delivery?.Address?.BuildingNum}, кв. {order?.Delivery?.Address?.ApartmentNum}, п. {order?.Delivery?.Address?.EntranceNum}";
        public static int GetProductSum(OrderDetailProduct? odp) => (odp?.Product?.Price ?? 0) * (odp?.Qty ?? 0);
        public static int GetServiceSum(OrderDetailService? ods) => (ods?.Service?.Price ?? 0) * (ods?.Qty ?? 0);

        public static string GetStatusCssClass(Order? order)
        {
            if (order != null)
            {
                switch (order.Status)
                {
                    case "Не назначен":
                        return "text-bg-secondary";
                    case "Назначен":
                        return "text-bg-primary";
                    case "Выполняется":
                        return "text-bg-warning";
                    case "Произошло ЧП":
                        return "text-bg-danger";
                    case "Завершен":
                        return "text-bg-success";
                    default:
                        return "";
                }
            }
            return "";
        }
    }
}
