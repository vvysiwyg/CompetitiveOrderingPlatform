using OrdersShared.POCOs;
using System.Diagnostics.Metrics;

namespace OrdersWebApp.Features.Couriers
{
    public static class Helper
    {
        public static string GetFullName(Courier? courier)
            => $"{courier?.Emp?.LastName} {courier?.Emp?.FirstName} {courier?.Emp?.MiddleName}";

        public static string GetSuccessOrdersPct(Courier courier)
        {
            if ((courier.NumberOfSuccessOrders == null && courier.NumberOfFailedOrders == null) ||
                (courier.NumberOfSuccessOrders == 0 && courier.NumberOfFailedOrders == 0))
                return "0";

            return ((double)courier.NumberOfSuccessOrders / (double)(courier.NumberOfSuccessOrders + courier.NumberOfFailedOrders) * 100.0).ToString("N2");
        }

        public static string GetMonthlySuccessOrdersPct(CourierHistory courierHistory) 
        {
            if ((courierHistory.NumberOfSuccessOrders == null && courierHistory.NumberOfFailedOrders == null) ||
                (courierHistory.NumberOfSuccessOrders == 0 && courierHistory.NumberOfFailedOrders == 0))
                return "0";

            return ((double)courierHistory.NumberOfSuccessOrders / (double)(courierHistory.NumberOfSuccessOrders + courierHistory.NumberOfFailedOrders) * 100.0).ToString("N2"); 
        }

        public static string GetMonthlySuccessOrdersPct(List<CourierHistory>? courierHistory)
        {
            double totalNumberOfSuccessOrders = GetTotalNumberOfSuccessOrders(courierHistory);
            double totalNumberOfFailedOrders = GetTotalNumberOfFailedOrders(courierHistory);

            if (totalNumberOfSuccessOrders == 0 && totalNumberOfFailedOrders == 0)
                return "0";

            return (totalNumberOfSuccessOrders / (totalNumberOfSuccessOrders + totalNumberOfFailedOrders) * 100.0).ToString("N2");
        }

        public static int GetTotalNumberOfSuccessOrders(List<CourierHistory>? courierHistories)
        {
            int totalNumber = 0;
            foreach (CourierHistory ch in courierHistories ?? new List<CourierHistory>())
                totalNumber += ch.NumberOfSuccessOrders ?? 0;
            return totalNumber;
        }

        public static int GetTotalNumberOfFailedOrders(List<CourierHistory>? courierHistories)
        {
            int totalNumber = 0;
            foreach (CourierHistory ch in courierHistories ?? new List<CourierHistory>())
                totalNumber += ch.NumberOfFailedOrders ?? 0;
            return totalNumber;
        }

        public static double GetSummirizedIncome(List<CourierHistory>? courierHistories)
        {
            double sum = 0.0;
            foreach (CourierHistory ch in courierHistories ?? new List<CourierHistory>())
                sum += ch.Income ?? 0;
            return sum;
        }

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

        /*public enum Months
        {
            January,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }*/
    }
}
