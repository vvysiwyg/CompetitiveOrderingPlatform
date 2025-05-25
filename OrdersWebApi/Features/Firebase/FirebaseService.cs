using FirebaseAdmin.Messaging;

namespace OrdersWebApi.Features.Firebase
{
    public class FirebaseService
    {
        public async Task SendNotification(NotificationRequest request)
        {
            Console.WriteLine("Creating message");
            var message = new Message()
            {
                Notification = new Notification
                {
                    Title = request.Title,
                    Body = request.Body
                },
                Token = request.DeviceToken
            };
            Console.WriteLine("Message created");

            Console.WriteLine("Sending notificaion...");
            var result = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            Console.WriteLine("Notificaion sent " + result);
        }
    }
}
