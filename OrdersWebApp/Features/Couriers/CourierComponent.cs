using Microsoft.AspNetCore.Components;
using OrdersShared.POCOs;

namespace OrdersWebApp.Features.Couriers
{
    public partial class CourierComponent
    {
        [Inject] private NavigationManager navigationManager { get; set; }

        [Parameter, EditorRequired] public Courier Courier { get; set; }

        private void ToCourierDetail(int id)
        {
            navigationManager.NavigateTo($"courier/{id}");
        }
    }
}
