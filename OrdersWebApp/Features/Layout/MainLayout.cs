using Microsoft.AspNetCore.Components;
using OrdersShared.Features.Geolocation;
using OrdersWebApp.Features.Geolocation;
using OrdersWebApp.Features.Hub;

namespace OrdersWebApp.Features.Layout
{
    public partial class MainLayout
    {
        [Inject] private GeolocationService GeolocationService { get; set; }

        [Inject] private HubService HubService { get; set; }

        public Coordinates CurrentCoordinates { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CurrentCoordinates = new Coordinates();
            GeolocationService.OnPositionReceived += UpdatePosition;
            await base.OnInitializedAsync();
            //await Task.Delay(2000);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            await GeolocationService.WatchPositionAsync();
        }

        private void UpdatePosition(double lat, double lon)
        {
            if (HubService.UserId == string.Empty)
                return;

            if (!GeolocationService.DoUpdateGeolocation(CurrentCoordinates, new Coordinates(lat, lon)))
                return;

            CurrentCoordinates.Latitude = lat;
            CurrentCoordinates.Longitude = lon;
            HubService.UpdateUserCoordinates(CurrentCoordinates);
        }
    }
}