using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using OrdersShared.Features.Geolocation;
using OrdersWebApi.Features.Firebase;
using OrdersWebApi.Features.Geolocation;

namespace OrdersWebApi.Features.Hub
{
    public class UserHub: Microsoft.AspNetCore.SignalR.Hub
    {
        private static Users OnlineUsers = new();

        private GeolocationService _geolocationService;

        private static ConcurrentDictionary<int, Dictionary<string, bool>> CourierResponses = new();

        private static ConcurrentDictionary<int, bool> HandledOrders = new();                           // Перечень назначенных заказов можно вынести в бд

        private FirebaseService _firebaseService;

        public UserHub(GeolocationService geolocationService, FirebaseService firebaseService)
        {
            _geolocationService = geolocationService;
            _firebaseService = firebaseService;
        }

        public override Task OnConnectedAsync()
        {
            string? id = Context.GetHttpContext()?.Request.Query["userId"];
            
            if (!string.IsNullOrEmpty(id))
            {
                OnlineUsers.Add(new User(id, Context.ConnectionId));
            }

            Console.WriteLine($"Connection {Context.ConnectionId} established. User {id} connected.");

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            User? removedUser = OnlineUsers.Remove(Context.ConnectionId);

            if (removedUser != null)
            {
                Console.WriteLine($"Connection {removedUser.ConnectionId} ended. User {removedUser.Auth0Id} disconnected.");
            }

            return base.OnDisconnectedAsync(exception);
        }

        public void UpdateUserCoordinates(Coordinates newCoordinates)
        {
            User? user = OnlineUsers.FirstOrDefault(f => f.ConnectionId == Context.ConnectionId);

            if (user != null)
            {
                user.Latitude = newCoordinates.Latitude;
                user.Longitude = newCoordinates.Longitude;
            }
        }

        public async Task GetNearestCouriers(int orderIssuePointId)
        {
            if (OnlineUsers.Count == 0)
                throw new Exception("There is no online users on the server");

            // Более лаконичный вариант, надо проверить
            //Dictionary<string, User> auth0BasedDictionary =
            //    new Dictionary<string, User>(OnlineUsers.Select(s => new KeyValuePair<string, User>(s.Key, s.Value)));

            HashSet<CourierGeolocation> response = await _geolocationService.GetNearestCouriers(orderIssuePointId, OnlineUsers);
            await Clients.Caller.SendAsync("GetNearestCouriers", response);
        }

        public async Task SetFCMToken(string fcmToken)
        {
            Console.WriteLine($"SetFCMToken. {fcmToken}");
            User? foundUser = OnlineUsers.FirstOrDefault(f => f.ConnectionId == Context.ConnectionId);

            if (foundUser == null || foundUser?.FcmToken != string.Empty)
                return;

            foundUser.FcmToken = fcmToken;
            Console.WriteLine($"Waiting 5 sec...");
            //Thread.Sleep(5 * 1000);
            //Console.WriteLine($"Done");
            //await _firebaseServerService.Test(new NotificationRequest()
            //{
            //    Body = "Test body",
            //    Title = "Test title",
            //    DeviceToken = fcmToken
            //});
        }

        public async Task RespondToOrder(int orderId, bool isAccepted)
        {
            Console.WriteLine($"Добавление {(isAccepted ? "положительного" : "отрицательного")} ответа курьера {Context.ConnectionId} на заказ {orderId}.");

            if (HandledOrders.ContainsKey(orderId) && HandledOrders[orderId] && isAccepted)
            {
                Console.WriteLine($"Заказ {orderId} уже обработан, отправка уведомления...");
                await Clients.Caller.SendAsync("LateOrderAcceptanceConfirm", "К сожалению, заказ был принят другим курьером.");
                Console.WriteLine($"Уведомление по заказу {orderId} отправлено");
            }

            Console.WriteLine($"{Context.ConnectionId}: Добавление или обновление CourierResponses...");
            CourierResponses.AddOrUpdate(orderId,
                key => new Dictionary<string, bool>()
                {
                    { Context.ConnectionId, isAccepted }
                },
                (key, oldValue) =>
                {
                    oldValue.Add(Context.ConnectionId, isAccepted);
                    return oldValue;
                });
            Console.WriteLine($"{Context.ConnectionId}: orderId: {orderId}, value: {string.Join(",", CourierResponses[orderId])}");
        }

        public static void AddHandledOrderId(int orderId)
        {
            HandledOrders.AddOrUpdate(orderId,
                key => true,
                (key, oldValue) => true);
        }

        public static bool CheckOrderAssigned(int orderId) => HandledOrders.ContainsKey(orderId) && HandledOrders[orderId];

        public static Dictionary<string, bool>? GetCourierResponses(int orderId)
            => CourierResponses.TryGetValue(orderId, out var responses) ? responses : null;

        public static Users GetOnlineUsers() => OnlineUsers;
    }
}
