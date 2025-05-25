namespace OrdersShared.POCOs;

public class OrderDetailProduct
{
    public int? Qty { get; set; }

    public int? OrderId {get;set;}

    public int? ProductId {get;set;}

    public Order? Order { get; set; }

    public Product? Product { get; set; }
}
