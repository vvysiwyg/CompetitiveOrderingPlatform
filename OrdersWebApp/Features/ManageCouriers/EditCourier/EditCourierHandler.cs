using MediatR;
using OrdersShared.Features.ManageCouriers.EditCourier;
using System.Net.Http.Json;

namespace OrdersWebApp.Features.ManageCouriers.EditCourier
{
    public class EditCourierHandler : IRequestHandler<EditCourierRequest, EditCourierRequest.Response>
    {
        private readonly HttpClient _httpClient;

        public EditCourierHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EditCourierRequest.Response> Handle(EditCourierRequest request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PutAsJsonAsync(EditCourierRequest.RouteTemplate, request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return new EditCourierRequest.Response(true);
            }
            else
            {
                return new EditCourierRequest.Response(false);
            }
        }
    }
}
