using Microsoft.AspNetCore.Components;
using OrdersShared.POCOs;

namespace OrdersWebApp.Features.Orders
{
    public partial class OrderComponent
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Parameter] public Order? Order { get;set; }

        private void ToOrderDetail(int id)
        {
            NavigationManager.NavigateTo($"order/{id}");
        }
    }
}
