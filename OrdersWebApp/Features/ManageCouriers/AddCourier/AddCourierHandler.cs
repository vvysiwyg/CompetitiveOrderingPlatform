using MediatR;
using OrdersShared.Features.ManageCouriers.AddCourier;
using System.Net.Http.Json;

namespace OrdersWebApp.Features.ManageCouriers.AddCourier
{
    public class AddCourierHandler : IRequestHandler<AddCourierRequest, AddCourierRequest.Response>
    {
        private readonly HttpClient _httpClient;

        public AddCourierHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AddCourierRequest.Response> Handle(AddCourierRequest request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync(AddCourierRequest.RouteTemplate, request, cancellationToken);
            
            if (response.IsSuccessStatusCode)
            {
                var id = await response.Content.ReadFromJsonAsync<int>(cancellationToken: cancellationToken);
                return new AddCourierRequest.Response(id);
            }
            else
            {
                return new AddCourierRequest.Response(-1);
            }
        }
    }
}
