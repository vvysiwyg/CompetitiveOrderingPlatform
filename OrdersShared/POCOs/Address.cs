namespace OrdersShared.POCOs;

public class Address
{
    public Address()
    {
        Deliveries = new HashSet<Delivery>();
    }
    
    public int Id { get; set; }

    public string? City { get; set; }

    public string? StreetName { get; set; }

    public string? BuildingNum { get; set; }

    public string? ApartmentNum { get; set; }

    public int? EntranceNum { get; set; }

    public int? IntercomeCode { get; set; }

    public virtual ICollection<Delivery> Deliveries { get; set; }
}
