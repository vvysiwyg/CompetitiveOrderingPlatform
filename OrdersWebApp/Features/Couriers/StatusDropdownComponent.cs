using Microsoft.AspNetCore.Components;
using OrdersShared.POCOs;

namespace OrdersWebApp.Features.Couriers
{
    public partial class StatusDropdownComponent
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Parameter, EditorRequired] public ICollection<CourierOrder> CourierOrders { get; set; }

        public ICollection<CourierOrder>? FilteredCourierOrders { get; set; }

        public List<string>? Filters { get; set; }

        public string? CssClass { get; set; }

        public bool Visible { get; set; }

        private void FilterBlockVisibilityChange()
        {
            Visible = !Visible;
            CssClass = Visible ? "visible" : "invisible";
        }

        protected override void OnInitialized()
        {
            Visible = false;
            CssClass = "invisible";
            FilteredCourierOrders = new List<CourierOrder>();

            foreach (CourierOrder co in CourierOrders)
                FilteredCourierOrders.Add(co);
            Filters = new List<string>();
        }

        private void ToOrderDetail(int id)
        {
            NavigationManager.NavigateTo($"order/{id}");
        }

        public void AssignedCheckboxClicked(ChangeEventArgs e)
        {
            bool check;
            if (bool.TryParse(e.Value?.ToString(), out check))
            {
                if (check)
                    Filters?.Add("Назначен");
                else
                    Filters?.Remove("Назначен");

                UpdateFilteredCourierOrdersCollection();
                StateHasChanged();
            }
        }

        public void IsPerformingCheckboxClicked(ChangeEventArgs e)
        {
            bool check;
            if (bool.TryParse(e.Value?.ToString(), out check))
            {
                if (check)
                    Filters?.Add("Выполняется");
                else
                    Filters?.Remove("Выполняется");

                UpdateFilteredCourierOrdersCollection();
                StateHasChanged();
            }
        }

        public void EmergencyCheckboxClicked(ChangeEventArgs e)
        {
            bool check;
            if (bool.TryParse(e.Value?.ToString(), out check))
            {
                if (check)
                    Filters?.Add("Произошло ЧП");
                else
                    Filters?.Remove("Произошло ЧП");

                UpdateFilteredCourierOrdersCollection();
                StateHasChanged();
            }
        }

        public void CompletedCheckboxClicked(ChangeEventArgs e)
        {
            bool check;
            if (bool.TryParse(e.Value?.ToString(), out check))
            {
                if (check)
                    Filters?.Add("Завершен");
                else
                    Filters?.Remove("Завершен");

                UpdateFilteredCourierOrdersCollection();
                StateHasChanged();
            }
        }

        public void UpdateFilteredCourierOrdersCollection()
        {
            FilteredCourierOrders?.Clear();
            if(Filters?.Count !=  0)
                foreach(CourierOrder co in CourierOrders)
                {
                    if (Filters?.Contains(co.Order?.Status ?? "") ?? false)
                        FilteredCourierOrders?.Add(co);
                }
            else
                foreach (CourierOrder co in CourierOrders)
                    FilteredCourierOrders?.Add(co);
        }
    }
}
