namespace OrdersShared.POCOs;

public class Service
{
    public Service()
    {
        OrderDetailServices = new HashSet<OrderDetailService>();
    }

    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Price { get; set; }

    public string? Description {get; set;}

    public ICollection<OrderDetailService> OrderDetailServices { get; set; }
}
