using MediatR;
using OrdersShared.Features.ManageCouriers.EditCourier;
using System.Net.Http.Json;

namespace OrdersWebApp.Features.ManageCouriers.EditCourier
{
    public class GetCourierForEditHandler : IRequestHandler<GetCourierForEditRequest, GetCourierForEditRequest.Response?>
    {
        private readonly HttpClient _httpClient;

        public GetCourierForEditHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetCourierForEditRequest.Response?> Handle(GetCourierForEditRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<GetCourierForEditRequest.Response>(GetCourierForEditRequest.RouteTemplate.Replace("{id}", request.id.ToString()));
            }
            catch (HttpRequestException)
            {
                return default!;
            }
        }
    }
}
