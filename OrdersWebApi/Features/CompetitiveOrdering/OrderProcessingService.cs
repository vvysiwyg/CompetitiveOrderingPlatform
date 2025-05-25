using Microsoft.AspNetCore.SignalR;
using OrdersWebApi.Features.Geolocation;
using OrdersWebApi.Features.Hub;

namespace OrdersWebApi.Features.CompetitiveOrdering
{
    public class OrderProcessingService : BackgroundService
    {
        private readonly ILogger<OrderProcessingService> _logger;
        private readonly IHubContext<UserHub> _hubContext;
        private Queue<int> OrderIds = new(new int[] { 1, 2, 3, 4, 5 });
        private readonly int _initialRadius = 5;
        private readonly int _radiusIncrement = 3;
        //private GeolocationService _geolocationService;

        public OrderProcessingService(
            ILogger<OrderProcessingService> logger,
            IHubContext<UserHub> hubContext/*,
            GeolocationService geolocationService*/)
        {
            _logger = logger;
            _hubContext = hubContext;
            //_geolocationService = geolocationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🚀 Запущен фоновый процесс назначения заказов.");

            while (!stoppingToken.IsCancellationRequested)
            {
                int onlineCouriersCount = UserHub.GetOnlineUsers().Count;
                _logger.LogInformation($"Количество онлайн курьеров: {onlineCouriersCount}");

                if (onlineCouriersCount > 1)
                {
                    if (OrderIds.TryDequeue(out int orderId))
                    {
                        _logger.LogInformation($"Начинается обработка заказа: {orderId}");
                        await ProcessOrderAsync(orderId, stoppingToken);
                    }
                    else
                    {
                        _logger.LogInformation("Заказы кончились, ожидание новых заказов...");
                        await Task.Delay(5 * 1000, stoppingToken);
                    }
                }
                else
                {
                    await Task.Delay(5 * 1000, stoppingToken);
                }
            }
        }

        private async Task ProcessOrderAsync(int orderId, CancellationToken stoppingToken)
        {
            int currentRadius = _initialRadius;
            bool assigned = false;

            while (!assigned && currentRadius < 6 && !stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"🔍 Ищем курьеров в радиусе {currentRadius} км для заказа {orderId}");

                List<string> availableCouriers = FindAvailableCouriers(orderId, currentRadius);
                if (availableCouriers.Any())
                {
                    assigned = await SendOrderToCouriers(orderId, availableCouriers);
                }

                if (!assigned)
                {
                    _logger.LogInformation($"📢 Расширяем радиус поиска до {currentRadius + _radiusIncrement} км.");
                    currentRadius += _radiusIncrement;
                    await Task.Delay(5 * 1000, stoppingToken);
                }
            }

            if (!assigned)
            {
                _logger.LogWarning($"❌ Заказ {orderId} не был принят. Оповещаем оператора.");
                // Оповещаем оператора (реализуйте через SignalR или email/SMS)
            }
            else
            {
                _logger.LogWarning($"Заказ {orderId} был принят.");
            }
        }

        // TODO реализовать поиск курьеров
        private List<string> FindAvailableCouriers(int orderId, int radiusMultiplier)
        {
            return UserHub.GetOnlineUsers().OnlineUsers.Select(s => s.ConnectionId).ToList();
            //Users onlineUsers = UserHub.GetOnlineUsers();
            //int radius = 5;
            //var result = await _geolocationService.GetNearestCouriers(0, onlineUsers);  // TODO: реализовать получение id ПВЗ из БД
            //List<string> response = new List<string>();

            //foreach (var courier in result)
            //{
            //    User? foundUser = onlineUsers.FindByAuth0(courier.Auth0Id);

            //    if(foundUser != null && courier.Distance >= radius * (radiusMultiplier - 1) && courier.Distance <= radius * radiusMultiplier)
            //        response.Add(foundUser.ConnectionId);
            //}

            //return response;
        }

        private async Task<bool> SendOrderToCouriers(int orderId, List<string> courierConnections)
        {
            var tcs = new TaskCompletionSource<bool>();

            _ = Task.Run(async () =>
            {
                var response = await WaitForCourierResponse(orderId, courierConnections.Count);
                if (response != null)
                {
                    _logger.LogInformation($"✅ Курьер {response} принял заказ {orderId}.");
                    tcs.TrySetResult(true);
                }
                else
                {
                    tcs.TrySetResult(false);
                }
            });

            _logger.LogInformation($"Отправка предложений на заказ {orderId} следующим курьерам: {string.Join(",", courierConnections)}");
            await _hubContext.Clients.Clients(courierConnections).SendAsync("NewOrder", orderId);
            _logger.LogInformation($"Предлжения по заказу {orderId} отправлены");
            return await tcs.Task;
        }

        private async Task<string?> WaitForCourierResponse(int orderId, int couriersCount)
        {
            var tcs = new TaskCompletionSource<string?>();
            int responseCount = 0;
            DateTime currentDateTime = DateTime.UtcNow;     // Нужно для того, чтобы избежать бесконечного цикла

            // Пока пришли не все ответы или не прошло еще много времени
            while (responseCount != couriersCount && DateTime.UtcNow.Subtract(currentDateTime).TotalMinutes < 2)
            {
                var courierResponses = UserHub.GetCourierResponses(orderId);

                if (courierResponses == null)
                {
                    _logger.LogInformation("1: Ожидание 0.5 секунды перед очередной проверкой...");
                    await Task.Delay(500);
                    _logger.LogInformation("1: Продолжение проверки");
                    continue;
                }

                responseCount = courierResponses.Count;
                var acceptedCouriers = courierResponses.Where(w => w.Value);
                var firstAccept = acceptedCouriers.FirstOrDefault(f => f.Value);
                var otherAccepts = acceptedCouriers.Where(w => w.Key != firstAccept.Key).Select(s => s.Key);

                // Если получили согласие курьера на заказ
                if (firstAccept.Value)
                {
                    _logger.LogInformation($"Получили согласие курьера {firstAccept.Key} на заказ {orderId}, отправка уведомления...");
                    await _hubContext.Clients.Client(firstAccept.Key).SendAsync("OrderAcceptanceConfirm", "Заказ был успешно принят!");
                    _logger.LogInformation($"Уведомление по заказу {orderId} отправлено");

                    _logger.LogInformation($"otherAccepts.Count = {otherAccepts.Count()}");
                    if (otherAccepts.Count() > 0)
                    {
                        _logger.LogInformation($"Отправляем не успевшим курьерам уведомление по заказу {orderId}...");
                        await _hubContext.Clients.Clients(otherAccepts).SendAsync("LateOrderAcceptanceConfirm", "К сожалению, заказ был принят другим курьером.");
                        _logger.LogInformation($"Уведомление по заказу {orderId} отправлено");
                    }

                    _logger.LogInformation($"Фиксируем факт окончания обработки заказа {orderId}...");
                    UserHub.AddHandledOrderId(orderId);

                    //if (CourierHub.CheckOrderAssigned(orderId))
                    //    _logger.LogInformation("Факт зафиксирован");
                    //else
                    //    _logger.LogInformation("Факт не зафиксирован");

                    tcs.TrySetResult(firstAccept.Key);
                    return await tcs.Task;
                }

                _logger.LogInformation("2: Ожидание 0.5 секунды перед очередной проверкой...");
                await Task.Delay(500);
                _logger.LogInformation("2: Продолжение проверки");
            }

            _logger.LogInformation("Все курьеры отказались от заказа");
            tcs.TrySetResult(null);

            return await tcs.Task;
        }
    }
}
