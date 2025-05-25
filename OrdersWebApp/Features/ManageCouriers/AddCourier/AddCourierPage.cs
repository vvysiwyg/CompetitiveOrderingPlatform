using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OrdersShared.Features.ManageCouriers.AddCourier;
using OrdersShared.Features.ManageCouriers.Shared;
using OrdersWebApp.Features.ManageCouriers.Shared;

namespace OrdersWebApp.Features.ManageCouriers.AddCourier
{
    public partial class AddCourierPage
    {
        [Inject] IMediator Mediator { get; set; }

        private CourierForm _courierForm = default!;
        private bool _submitSuccessful;
        private string? _errorMessage;

        private async Task SubmitNewTrail(CourierDto courier, IBrowserFile? image)
        {
            var response = await Mediator.Send(new AddCourierRequest(courier));
            if (response.id == -1)
            {
                _submitSuccessful = false;
                _errorMessage = "При сохранении данных курьера произошла ошибка";
                StateHasChanged();
                return;
            }

            if (image is null)
            {
                _submitSuccessful = true;
                _courierForm.ResetForm();
                StateHasChanged();
                return;
            }

            _submitSuccessful = await ProcessImage(response.id, image);

            if (_submitSuccessful)
            {
                _courierForm.ResetForm();
            }

            StateHasChanged();
        }

        private async Task<bool> ProcessImage(int id, IBrowserFile img)
        {
            var imageUploadResponse = await Mediator.Send(new UploadCourierImageRequest(id, img));

            if (string.IsNullOrWhiteSpace(imageUploadResponse.imageName))
            {
                _errorMessage = "Данные курьера успешно сохранены, но при загрузке изображения произошла ошибка";
                return false;
            }

            return true;
        }
    }
}
