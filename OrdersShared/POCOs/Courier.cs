namespace OrdersShared.POCOs
{
    public class Courier
    {
        public Courier()
        {
            CourierHistories = new HashSet<CourierHistory>();
            CourierOrders = new HashSet<CourierOrder>();
        }

        public int Id { get; set; }

        public int? NumberOfSuccessOrders { get; set; }

        public int? NumberOfFailedOrders { get; set; }

        public int? EmpId { get; set; }

        public string? Profile { get; set; }

        public ProfileEnum? ProfileType
        {
            get
            {
                if (string.IsNullOrEmpty(Profile))
                    return null;

                switch (Profile)
                {
                    case "walking":
                        return ProfileEnum.Walking;
                    case "bike":
                        return ProfileEnum.Bike;
                    case "driving":
                        return ProfileEnum.Driving;
                    default:
                        return null;
                }
            }
        }

        public virtual Emp? Emp { get; set; }

        public virtual ICollection<CourierHistory> CourierHistories { get; set; }

        public virtual ICollection<CourierOrder> CourierOrders { get; set; }
    }

    public enum ProfileEnum
    {
        Walking,
        Bike,
        Driving
    }
}
