using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using OrdersShared.Features.Geolocation;
using OrdersWebApp.Features.Orders.Modals;

namespace OrdersWebApp.Features.Hub
{
    public class HubService
    {
        private readonly NavigationManager _navigation;

        private HubConnection? HubConnection { get; set; }

        public string UserId { get; set; } = string.Empty;

        private Dictionary<string, Func<IDisposable?>> SubscriptionNameDelayedAction { get; set; }

        public HubService(NavigationManager navigation) 
        {
            _navigation = navigation;
            SubscriptionNameDelayedAction = new Dictionary<string, Func<IDisposable?>>();
        }

        public async Task CreateHubConnection(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return;

            UserId = userId;
            HubConnection = new HubConnectionBuilder()
                    .WithUrl(_navigation.ToAbsoluteUri($"/userhub?userId={UserId}"))
                    .WithAutomaticReconnect()
                    .Build();

            await HubConnection.StartAsync();

            try
            {
                foreach (var action in SubscriptionNameDelayedAction.Values)
                {
                    action.Invoke();
                }
            }
            finally
            {
                SubscriptionNameDelayedAction.Clear();
            }
        }

        public async Task CloseHubConnection()
        {
            if (HubConnection is not null)
            {
                UserId = string.Empty;
                await HubConnection.DisposeAsync();
            }
        }

        public void UpdateUserCoordinates(Coordinates newCoordinates)
        {
            if (HubConnection == null)
                return;

            HubConnection.InvokeAsync("UpdateUserCoordinates", newCoordinates);
        }

        public async Task GetNearestCouriers(int orderIssuePointId)
        {
            if (HubConnection == null)
                return;

            await HubConnection.InvokeAsync("GetNearestCouriers", orderIssuePointId);
        }

        public async Task RespondToOrder(int orderId, bool isAccepted)
        {
            if (HubConnection == null)
                return;

            await HubConnection.SendAsync("RespondToOrder", orderId, isAccepted);
        }

        public void SetFCMToken(string fcmToken)
        {
            if (HubConnection == null)
                return;

            HubConnection.SendAsync("SetFCMToken", fcmToken);
        }

        public IDisposable? Subscribe<T>(string subscriptionName, Action<T> messageCallback)
        {
            Func<IDisposable?> action = () =>
                HubConnection.On(subscriptionName,
                    (T message) =>
                    {
                        messageCallback(message);
                    });

            if (HubConnection == null)
            {
                if(!SubscriptionNameDelayedAction.ContainsKey(subscriptionName))
                    SubscriptionNameDelayedAction.Add(subscriptionName , action);

                return null;
            }

            return action.Invoke();
        }
    }
}
