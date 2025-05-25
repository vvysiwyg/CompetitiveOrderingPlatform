namespace OrdersShared.POCOs;

public class Delivery
{
    public Delivery()
    {
        Orders = new HashSet<Order>();
    }

    public int Id { get; set; }

    public DateOnly? Date { get; set; }

    public string? Commentary { get; set; }

    public bool? CallInHour { get; set; }

    public int? AddressId {get;set;}

    public virtual Address? Address { get; set; }

    public virtual ICollection<Order> Orders { get; set; }
}
