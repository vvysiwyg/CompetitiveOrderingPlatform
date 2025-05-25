namespace OrdersWebApi.Features.Hub
{
    public class Users
    {
        public List<User> OnlineUsers { get; set; }

        public int Count 
        {
            get
            {
                return OnlineUsers.Count;
            }
        }

        public Users() 
        {
            OnlineUsers = new List<User>();
        }

        public void Add(User user) => OnlineUsers.Add(user);

        public User? Remove(string connectionId)
        {
            User? removeUser = OnlineUsers.FirstOrDefault(f => f.ConnectionId == connectionId);

            if (removeUser != null)
            {
                OnlineUsers.Remove(removeUser);
                return removeUser;
            }

            return null;
        }

        public User? FindByAuth0(string auth0Id) => OnlineUsers.FirstOrDefault(f => f.Auth0Id == auth0Id);

        public User? FirstOrDefault(Func<User, bool> predicate) => OnlineUsers.FirstOrDefault(predicate);
    }
}
