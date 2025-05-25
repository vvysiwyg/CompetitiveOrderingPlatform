namespace OrdersWebApi.Features.Hub
{
    public class User
    {
        public string Auth0Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string FcmToken { get; set; }

        public string ConnectionId { get; set; }

        public User(string auth0Id, string connectionId)
        {
            Auth0Id = auth0Id;
            ConnectionId = connectionId;
            Latitude = 0;
            Longitude = 0;
            FcmToken = string.Empty;
        }

        public User(string auth0Id)
        {
            Auth0Id = auth0Id;
            Latitude = 0;
            Longitude = 0;
            FcmToken = string.Empty;
            ConnectionId = string.Empty;
        }

        public User()
        {
            Auth0Id = string.Empty;
            Latitude = 0;
            Longitude = 0;
            FcmToken = string.Empty;
            ConnectionId = string.Empty;
        }
    }
}
