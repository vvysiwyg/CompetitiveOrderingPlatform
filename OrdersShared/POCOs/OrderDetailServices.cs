namespace OrdersShared.POCOs;

public class OrderDetailService
{
    public int? Qty { get; set; }

    public int? OrderId {get;set;}

    public int? ServiceId {get;set;}

    public Order? Order { get; set; }

    public Service? Service { get; set; }
}