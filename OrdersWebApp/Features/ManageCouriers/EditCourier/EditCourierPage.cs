using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using OrdersShared.Features.ManageCouriers.Shared;
using OrdersShared.Features.ManageCouriers.EditCourier;

namespace OrdersWebApp.Features.ManageCouriers.EditCourier
{
    public partial class EditCourierPage
    {
        [Inject] IMediator Mediator { get; set; }

        private bool _isLoading;
        private bool _submitSuccessful;
        private string? _errorMessage;
        private CourierDto _courier = new CourierDto();

        [Parameter] public int Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;

            var response = await Mediator.Send(new GetCourierForEditRequest(Id));

            if (response.courier is not null)
            {
                _courier.Id = Id;
                _courier.FirstName = response.courier.FirstName;
                _courier.LastName = response.courier.LastName;
                _courier.MiddleName = response.courier.MiddleName;
                _courier.Email = response.courier.Email;
                _courier.MobilePhone = response.courier.MobilePhone;
                _courier.Image = response.courier.Image;
            }
            else
            {
                _errorMessage = "При загрузке данных курьера произошла ошибка";
            }

            _isLoading = false;
        }

        private async Task SubmitEditCourier(CourierDto courier, IBrowserFile? image)
        {
            var response = await Mediator.Send(new EditCourierRequest(courier));

            if (!response.isSuccess)
            {
                _submitSuccessful = false;
                _errorMessage = "При сохранении данных курьера произошла ошибка";
            }
            else
            {
                _courier.FirstName = courier.FirstName;
                _courier.LastName = courier.LastName;
                _courier.MiddleName = courier.MiddleName;
                _courier.Email = courier.Email;
                _courier.MobilePhone = courier.MobilePhone;

                _submitSuccessful = true;

                if (courier.ImageAction == ImageAction.Add) _submitSuccessful = await ProcessImage(courier.Id, image!);
                if (courier.ImageAction == ImageAction.Remove) _courier.Image = null;
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

            _courier.Image = imageUploadResponse.imageName;
            return true;
        }
    }
}
