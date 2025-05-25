namespace OrdersShared.POCOs
{
    public class CourierOrder
    {
        public double? Cost { get; set; }

        public int? OrderId { get; set; }

        public int? CourierId { get; set; }

        public Order? Order { get; set; }

        public Courier? Courier { get; set; }
    }
}
