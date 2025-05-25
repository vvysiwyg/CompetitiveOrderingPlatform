using MediatR;
using OrdersShared.Features.Couriers;
using OrdersShared.POCOs;
using System.Net.Http.Json;

namespace OrdersWebApp.Features.Couriers.Handlers
{
    public class GetOIPSHandler : IRequestHandler<GetOIPSRequest, GetOIPSRequest.Response>
    {
        private readonly HttpClient _httpClient;

        public GetOIPSHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetOIPSRequest.Response> Handle(GetOIPSRequest request, CancellationToken cancellationToken)
        {
            List<OrderIssuePoint>? response = await _httpClient.GetFromJsonAsync<List<OrderIssuePoint>?>(GetOIPSRequest.RouteTemplate);

            if (response is null)
                throw new Exception("Ошибка при вызове метода GetOIPSRequest");

            return new GetOIPSRequest.Response(response);
        }
    }
}
