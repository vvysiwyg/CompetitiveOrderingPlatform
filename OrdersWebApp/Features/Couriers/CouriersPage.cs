using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OrdersShared.POCOs;
using MediatR;
using OrdersShared.Features.Orders;
using OrdersShared.Features.ManageCouriers.DeleteCourier;

namespace OrdersWebApp.Features.Couriers
{
    public partial class CouriersPage
    {
        [Inject] IJSRuntime JS { get; set; }

        [Inject] IMediator Mediator { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        List<Courier>? couriers { get; set; }

        private void AddNewCourier()
        {
            NavigationManager.NavigateTo($"add-courier");
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await Mediator.Send(new GetCouriersRequest());
                couriers = result.couriers;
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("console.log", $"{ex.Message}\n{ex.StackTrace}");
            }
        }

        private async void DeleteCouriersTest()
        {
            var result = await Mediator.Send(new DeleteCourierRequest(new List<int>() { 12 }));
            await JS.InvokeVoidAsync("console.log", $"result is {result.areAllDeleted}");
        }
    }
}
