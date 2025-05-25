using Microsoft.JSInterop;
using OrdersShared.Features.Geolocation;

namespace OrdersWebApp.Features.Geolocation
{
    public class GeolocationService
    {
        private readonly IJSRuntime _jsRuntime;
        private DotNetObjectReference<GeolocationService> _objectReference;

        public event Action<double, double> OnPositionReceived;

        public GeolocationService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public GeolocationService() { }

        public async Task<int> WatchPositionAsync()
        {
            _objectReference = DotNetObjectReference.Create(this);
            return await _jsRuntime.InvokeAsync<int>("geolocationInterop.watchPosition", _objectReference);
        }

        /// <summary>
        /// Если первые 3 символа после запятой в долготе или широте отличаются, то необходимо обновить координаты юзера на сервере
        /// </summary>
        /// <param name="coord1"></param>
        /// <param name="coord2"></param>
        /// <returns></returns>
        public bool DoUpdateGeolocation(Coordinates coord1, Coordinates coord2)
        {
            int lat1 = (int)Math.Truncate(coord1.Latitude * 1000),
                lon1 = (int)Math.Truncate(coord1.Longitude * 1000),
                lat2 = (int)Math.Truncate(coord2.Latitude * 1000),
                lon2 = (int)Math.Truncate(coord2.Longitude * 1000);

            return lat1 != lat2 || lon1 != lon2;
        }

        [JSInvokable]
        public void ReceivePosition(double latitude, double longitude)
        {
            OnPositionReceived?.Invoke(latitude, longitude);
        }
    }
}
