namespace OrdersShared.POCOs;

public class Order
{
    public Order()
    {
        OrderDetailProducts = new HashSet<OrderDetailProduct>();
        OrderDetailServices = new HashSet<OrderDetailService>();
        CourierOrders = new HashSet<CourierOrder>();
    }

    public int Id { get; set; }

    public string? Number { get; set; }

    public int? Sum { get; set; }

    public DateTime? CreationDate { get; set; }

    public string? PaymentType { get; set; }

    public string? Status { get; set; }

    public int? DeliveryId {get;set;}

    public int? CustomerId {get;set;}

    public virtual Delivery? Delivery { get; set; }

    public virtual Customer? Customer { get; set; }

    public ICollection<OrderDetailProduct> OrderDetailProducts { get; set; }

    public ICollection<OrderDetailService> OrderDetailServices { get; set; }

    public ICollection<CourierOrder> CourierOrders { get; set; }
}
