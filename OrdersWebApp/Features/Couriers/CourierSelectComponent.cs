using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OrdersShared.POCOs;
using MediatR;
using OrdersShared.Features.Orders;

namespace OrdersWebApp.Features.Couriers
{
    public partial class CourierSelectComponent
    {
        [Inject] private IJSRuntime JS { get; set; }

        [Inject] IMediator Mediator { get; set; }

        [Parameter, EditorRequired] public bool IsOpen { get; set; }

        [Parameter, EditorRequired] public int OrderId { get; set; }

        [Parameter, EditorRequired] public EventCallback<LinkCourierWithOrderArgs> OnLinkBtnClick { get; set; }

        public double DeliverySum { get; set; }

        List<Courier>? Couriers { get; set; }

        public string? SelectedCourierId { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                if (IsOpen)
                {
                    await JS.InvokeVoidAsync("console.log", "CourierSelectComponent OnParametersSetAsync Invoked");
                    var result = await Mediator.Send(new GetCouriersRequest());
                    Couriers = result.couriers;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("console.log", $"{ex.Message}\n{ex.StackTrace}");
            }
        }

        private void FormDeliveryCost()
        {

        }

        private async Task LinkCourierWithOrder()
        {
            bool isLinked = false;
            string selectedCourierFullName = "";

            if (int.TryParse(SelectedCourierId, out int courierId))
            {
                var result = await Mediator.Send(new LinkCourierWithOrderRequest(courierId, OrderId, DeliverySum));
                isLinked = result.result;
                Courier? selectedCourier = Couriers?.FirstOrDefault(c => c.Id == courierId);
                if(selectedCourier != null)
                {
                    selectedCourierFullName = Helper.GetFullName(selectedCourier);
                }
            }

            if(isLinked)
                await JS.InvokeVoidAsync("console.log", "Успешно связано");
            else
                await JS.InvokeVoidAsync("console.log", "Связано неудачно");

            await OnLinkBtnClick.InvokeAsync(new LinkCourierWithOrderArgs
                (false, string.IsNullOrWhiteSpace(SelectedCourierId) ? "" : SelectedCourierId, selectedCourierFullName)
            );
        }
    }
}
