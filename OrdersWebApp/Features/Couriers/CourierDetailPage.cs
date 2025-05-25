using Microsoft.AspNetCore.Components;
using OrdersShared.POCOs;
using MediatR;
using OrdersShared.Features.Orders;

namespace OrdersWebApp.Features.Couriers
{
    public partial class CourierDetailPage
    {
        [Inject] IMediator Mediator { get; set; }

        [Parameter] public int Id { get; set; }

        private Courier? Courier { get; set; }

        private List<CourierHistory>? CurrentCourierHistories { get; set; }

        private DateTime SelectedStartMonth { get; set; } = DateTime.Now;

        private DateTime SelectedEndMonth { get; set; } = DateTime.Now;

        protected override void OnInitialized()
        {
            CurrentCourierHistories = new List<CourierHistory>();
        }

        protected override async Task OnInitializedAsync()
        {
            var result = await Mediator.Send(new GetCourierRequest(Id));
            Courier = result.courier;
            #region Test
            Courier.CourierHistories.Add(
                new CourierHistory
                {
                    Income = 252.5,
                    MeasurementDate = new DateOnly(2024, 2, 29),
                    NumberOfSuccessOrders = 21,
                    NumberOfFailedOrders = 1
                }
            );
            Courier.CourierHistories.Add(
                new CourierHistory
                {
                    Income = 384.5,
                    MeasurementDate = new DateOnly(2024, 1, 31),
                    NumberOfSuccessOrders = 19,
                    NumberOfFailedOrders = 0
                }
            );
            #endregion Test
        }

        private void FilterCourierHistory()
        {
            if (Courier != null) 
            {
                CurrentCourierHistories?.Clear();

                /*CurrentCourierHistories = Courier.CourierHistories.
                    Where(w => 
                    w.MeasurementDate.Month >= SelectedStartMonth.Month &&
                    w.MeasurementDate.Year >= SelectedStartMonth.Year &&
                    w.MeasurementDate.Month <= SelectedEndMonth.Month &&
                    w.MeasurementDate.Year <= SelectedEndMonth.Year).
                    ToList();*/
                CurrentCourierHistories?.AddRange(Courier.CourierHistories.
                    Where(w =>
                    w.MeasurementDate?.CompareTo(DateOnly.FromDateTime(SelectedStartMonth), true) >= 0 &&
                    w.MeasurementDate?.CompareTo(DateOnly.FromDateTime(SelectedEndMonth), true) <= 0));

                Console.WriteLine(SelectedStartMonth.ToString());
                Console.WriteLine(SelectedEndMonth.ToString());
                foreach (var ch in CurrentCourierHistories ?? new List<CourierHistory>())
                {
                    Console.WriteLine(ch.MeasurementDate.ToString());
                }

                StateHasChanged();
            }
        }
    }
    public static class DateOnlyExtension
    {
        public static int CompareTo(this DateOnly DateOnly, DateOnly value, bool setToFirstDayOfMonth)
        {
            DateOnly newDateOnly = new DateOnly(DateOnly.Year, DateOnly.Month, setToFirstDayOfMonth ? 1 : DateOnly.Day);
            return newDateOnly.CompareTo(value);
        }
    }
}
