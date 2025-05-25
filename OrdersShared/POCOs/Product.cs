namespace OrdersShared.POCOs;

public class Product
{
    public Product()
    {
        OrderDetailProducts = new HashSet<OrderDetailProduct>();
    }

    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Price { get; set; }

    public string? Description {get; set;}

    public int? Code { get; set; }

    public string? CategoryName {get; set;}

    public ICollection<OrderDetailProduct> OrderDetailProducts { get; set; }
}
