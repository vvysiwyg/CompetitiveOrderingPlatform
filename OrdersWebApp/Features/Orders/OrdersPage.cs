using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OrdersShared.POCOs;
using MediatR;
using OrdersShared.Features.Orders;

namespace OrdersWebApp.Features.Orders
{
    public partial class OrdersPage
    {
        [Inject] IJSRuntime JS { get; set; }

        [Inject] IMediator Mediator { get; set; }

        [Parameter] public string Status 
        { 
            get 
            {
                if (_status == null || string.IsNullOrEmpty(_status))
                    return "notAssigned-orders";

                return _status;
            }
            set 
            { 
                _status = value; 
            }
        }

        private string _status;

        public List<Order> orders;

        public List<Order> TestOrders { get; set; }

        private string GetPageTitle()
        {
            switch(Status)
            {
                case "active-orders":
                    return "Выполняемые заказы";
                case "archive":
                    return "Архивные заказы";
                case "assigned-orders":
                    return "Назначенные заказы";
                default:
                    return "Не назначенные заказы";
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                var result = await Mediator.Send(new GetOrdersRequest(Status));
                orders = result.orders;
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("console.log", $"{ex.Message}\n{ex.StackTrace}");
            }

            await base.OnParametersSetAsync();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            TestOrders = new List<Order>
            {
                new Order
                {
                    Id = 2,
                    CreationDate = new DateTime(2024, 1, 19, 21, 12, 0),
                    Sum = 65190,
                    Status = "Не назначен",
                    DeliveryId = 2,
                    Delivery = new Delivery
                    {
                        Id = 2,
                        AddressId = 2,
                        Address = new Address
                        {
                            Id = 2,
                            City = "Краснодар",
                            StreetName = "Московская",
                            BuildingNum = "14",
                            ApartmentNum = "12",
                            EntranceNum = 1
                        }
                    }
                },
                new Order
                {
                    Id = 3,
                    CreationDate = new DateTime(2024, 1, 17, 16, 9, 0),
                    Sum = 59961,
                    Status = "Не назначен",
                    DeliveryId = 3,
                    Delivery = new Delivery
                    {
                        Id = 3,
                        AddressId = 3,
                        Address = new Address
                        {
                            Id = 3,
                            City = "Краснодар",
                            StreetName = "Офицерская",
                            BuildingNum = "44",
                            ApartmentNum = "35",
                            EntranceNum = 3
                        }
                    }
                },
                new Order
                {
                    Id = 4,
                    CreationDate = new DateTime(2024, 1, 17, 8, 55, 0),
                    Sum = 7999,
                    Status = "Не назначен",
                    DeliveryId = 4,
                    Delivery = new Delivery
                    {
                        Id = 4,
                        AddressId = 4,
                        Address = new Address
                        {
                            Id = 4,
                            City = "Краснодар",
                            StreetName = "Зиповская",
                            BuildingNum = "36",
                            ApartmentNum = "40",
                            EntranceNum = 2
                        }
                    }
                },
            };
        }
    }
}