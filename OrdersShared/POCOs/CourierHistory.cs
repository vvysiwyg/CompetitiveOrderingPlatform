namespace OrdersShared.POCOs
{
    public class CourierHistory
    {
        public int Id { get; set; }

        public int? NumberOfSuccessOrders { get; set; }

        public int? NumberOfFailedOrders { get; set; }

        public double? Income { get; set; }

        public DateOnly? MeasurementDate { get; set; }

        public int? CourierId { get; set; }

        public virtual Courier? Courier { get; set; }
    }
}
