using Microsoft.EntityFrameworkCore;
using OrdersShared.Features.Geolocation;
using OrdersShared.POCOs;
using OrdersWebApi.Features.Hub;
using System.Text.Json;

namespace OrdersWebApi.Features.Geolocation
{
    public class GeolocationService
    {
        private OrdersContext _ordersContext;
        private string osrmApiBaseUrl = "http://router.project-osrm.org/route/v1/{profile}/{startLon},{startLat};{endLon},{endLat}?overview=full";

        public GeolocationService(OrdersContext ordersContext) 
        {
            _ordersContext = ordersContext;
        }

        public async Task<HashSet<CourierGeolocation>> GetNearestCouriers(int orderIssuePointId, Users onlineUsers)
        {
            OrderIssuePoint orderIssuePoint = 
                await _ordersContext.OrderIssuePoints.FirstOrDefaultAsync(f => f.Id == orderIssuePointId);

            if (orderIssuePoint == null)
                throw new Exception($"В БД отсутствует ПВЗ с таким id {orderIssuePointId}");

            Coordinates orderIssuePointCoordinates = 
                new Coordinates(orderIssuePoint.Latitude.Value, orderIssuePoint.Longitude.Value);
            var onlineCouriers = _ordersContext.Couriers.
                Include(i => i.Emp).
                Where(w => w.Emp.Position == "Курьер" && onlineUsers.FirstOrDefault(f => f.Auth0Id == w.Emp.Auth0Id) != null).
                Select(s => new
                {
                    Courier = s,
                    Latitude = onlineUsers.FirstOrDefault(f => f.Auth0Id == s.Emp.Auth0Id).Latitude,
                    Longitude = onlineUsers.FirstOrDefault(f => f.Auth0Id == s.Emp.Auth0Id).Longitude
                }).
                ToHashSet();
            CourierGeolocation nearestWalkingCourier = new CourierGeolocation(),
                nearestBikeCourier = new CourierGeolocation(),
                nearestDrivingCourier = new CourierGeolocation();
            HashSet<CourierGeolocation> result = new HashSet<CourierGeolocation>();

            foreach(var onlineCourier in onlineCouriers)
            {
                Coordinates onlineCourierCoordinates = new Coordinates(onlineCourier.Latitude, onlineCourier.Longitude);
                Route? route = 
                    await CalculateRoute(onlineCourierCoordinates, orderIssuePointCoordinates, onlineCourier.Courier.Profile);

                if (route == null)
                    continue;

                switch(onlineCourier.Courier.ProfileType)
                {
                    case ProfileEnum.Walking:
                        Nearest(route, nearestWalkingCourier, onlineCourier.Courier);
                        break;
                    case ProfileEnum.Bike:
                        Nearest(route, nearestBikeCourier, onlineCourier.Courier);
                        break;
                    case ProfileEnum.Driving:
                        Nearest(route, nearestDrivingCourier, onlineCourier.Courier);
                        break;
                    default:
                        break;
                }
            }

            if (nearestWalkingCourier.Distance != double.MaxValue)
                result.Add(nearestBikeCourier);

            if (nearestBikeCourier.Distance != double.MaxValue)
                result.Add(nearestBikeCourier);

            if (nearestDrivingCourier.Distance != double.MaxValue)
                result.Add(nearestDrivingCourier);

            return result;
        }

        public void Nearest(Route route, CourierGeolocation nearestCourier, Courier courier)
        {
            if (route.Distance < nearestCourier.Distance)
            {
                nearestCourier.Distance = route.Distance;
                nearestCourier.DestinationTime = route.Duration;
                nearestCourier.CourierId = courier.Id;
                nearestCourier.FullName = $"{courier?.Emp?.LastName} {courier?.Emp?.FirstName} {courier?.Emp?.MiddleName}";
                nearestCourier.ProfileType = courier?.ProfileType;
                nearestCourier.Auth0Id = courier?.Emp?.Auth0Id;
            }
        }

        public async Task<Route?> CalculateRoute(Coordinates coord1, Coordinates coord2, string profile)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    string url = osrmApiBaseUrl.
                        Replace("{profile}", profile).
                        Replace("{startLon}", coord1.Longitude.ToString().Replace(",", ".")).
                        Replace("{startLat}", coord1.Latitude.ToString().Replace(",", ".")).
                        Replace("{endLon}", coord2.Longitude.ToString().Replace(",", ".")).
                        Replace("{endLat}", coord2.Latitude.ToString().Replace(",", "."));

                    // Отправка GET-запроса
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    // Чтение и обработка ответа
                    JsonSerializerOptions opts = new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string? jsonResponse = await response.Content.ReadAsStringAsync();
                    RouteResponse routeResponse = JsonSerializer.Deserialize<RouteResponse>(jsonResponse, opts);

                    if (routeResponse == null || routeResponse.Routes.Length == 0)
                        Console.WriteLine("Маршрут не найден.");

                    return routeResponse.Routes.FirstOrDefault();
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Ошибка запроса: {e.Message}");
                }
            }

            return null;
        }
    }
}
