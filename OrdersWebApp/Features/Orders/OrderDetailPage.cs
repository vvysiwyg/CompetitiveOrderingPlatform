using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OrdersShared.POCOs;
using OrdersWebApp.Features.Couriers;
using MediatR;
using OrdersShared.Features.Orders;

namespace OrdersWebApp.Features.Orders
{
    public partial class OrderDetailPage
    {
        [Inject] private IJSRuntime JS { get; set; }

        [Inject] IMediator Mediator { get; set; }

        [Parameter] public int Id {get;set;}

        public Order? Order { get;set; }

        public string SelectedCourierId { get; set; } = "";

        public string SelectedCourierFullName { get; set; } = "";

        private bool _isOpen = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await Mediator.Send(new GetOrderRequest(Id));
                Order = result.order;
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("console.log", $"{ex.Message}\n{ex.StackTrace}");
            }

            await base.OnInitializedAsync();
        }

        private void OpenOrCloseCourierSelectDialog(LinkCourierWithOrderArgs args)
        {
            _isOpen = args.isOpen;
            SelectedCourierId = args.selectedCourierId;
            SelectedCourierFullName = args.selectedCourierFullName;
        }

        private async Task UnlinkCourierWithOrder()
        {
            bool isUnlinked = false;

            if (int.TryParse(SelectedCourierId, out int courierId))
            {
                var result = await Mediator.Send(new UnlinkCourierWithOrderRequest(courierId.ToString(), Id.ToString()));
                isUnlinked = result.result;
                SelectedCourierId = "";
                SelectedCourierFullName = "";
            }

            if (isUnlinked)
                await JS.InvokeVoidAsync("console.log", "Успешно убрана связь");
            else
                await JS.InvokeVoidAsync("console.log", "Связь не убрана");

            StateHasChanged();
        }
    }
}