using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using OrdersWebApp.Validation;
using OrdersShared.Features.ManageCouriers.Shared;

namespace OrdersWebApp.Features.ManageCouriers.Shared
{
    public partial class CourierForm
    {
        private CourierDto _courier = new CourierDto();
        private IBrowserFile? _courierImage;
        private EditContext _editContext = default!;

        [Parameter, EditorRequired] public Func<CourierDto, IBrowserFile?, Task> OnSubmit { get; set; } = default!;
        [Parameter] public CourierDto? Courier { get; set; }


        public void ResetForm()
        {
            _courier = new CourierDto();
            _editContext = new EditContext(_courier);
            _editContext.SetFieldCssClassProvider(new BootstrapCssClassProvider());
            _courierImage = null;
        }

        protected override void OnParametersSet()
        {
            _editContext = new EditContext(_courier);
            _editContext.SetFieldCssClassProvider(new BootstrapCssClassProvider());

            if (Courier != null)
            {
                _courier.Id = Courier.Id;
                _courier.FirstName = Courier.FirstName;
                _courier.LastName = Courier.LastName;
                _courier.MiddleName = Courier.MiddleName;
                _courier.MobilePhone = Courier.MobilePhone;
                _courier.Email = Courier.Email;
                _courier.Image = Courier.Image;
                _courier.ImageAction = ImageAction.None;
            }
        }

        private void LoadCourierImage(InputFileChangeEventArgs e)
        {
            _courierImage = e.File;
            _courier.ImageAction = ImageAction.Add;
        }

        private void RemoveCourierImage()
        {
            _courier.Image = null;
            _courier.ImageAction = ImageAction.Remove;
        }

        private async Task SubmitForm() => await OnSubmit(_courier, _courierImage);
    }
}
