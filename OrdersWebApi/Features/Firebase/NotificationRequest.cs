namespace OrdersWebApi.Features.Firebase
{
    public class NotificationRequest
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public string DeviceToken { get; set; }

        //public string OrderNumber { get; set; }

        //public string DeliveryAddress { get; set; }

        //public string DeliveryTime { get; set; }

        //public string PickupPoint { get; set; }
    }
}
