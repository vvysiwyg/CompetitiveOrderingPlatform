using OrdersShared.POCOs;

namespace OrdersShared.Features.Geolocation
{
    public class CourierGeolocation
    {
        public double Distance { get; set; }

        public double DestinationTime { get; set; }

        public int CourierId { get; set; }

        public string FullName { get; set; }

        public ProfileEnum? ProfileType { get; set; }

        public string Auth0Id { get; set; }

        public CourierGeolocation()
        {
            Distance = double.MaxValue;
            DestinationTime = double.MaxValue;
            CourierId = -1;
            FullName = string.Empty;
            ProfileType = null;
            Auth0Id = string.Empty;
        }

        //public double GetDistanceInKm() => (double)(Distance / 1000.0);

        //public double GetDestinationTimeInHours() => DestinationTime / 60.0;
    }
}
