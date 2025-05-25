namespace OrdersShared.POCOs;

public class Emp
{
    public Emp()
    {
        Couriers = new HashSet<Courier>();
    }
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Position { get; set; }

    public string? Photo { get; set; }

    public string? MobilePhone { get; set; }

    public string? Auth0Id { get; set; }

    public ICollection<Courier> Couriers { get; set; }
}
