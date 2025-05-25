namespace OrdersShared.POCOs;

public class Customer
{
    public Customer()
    {
        Orders = new HashSet<Order>();
    }

    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? Phone { get; set; }

    public string? Email {get; set;}

    public string? CustomerType {get; set;}

    public ICollection<Order> Orders { get; set; }
}
