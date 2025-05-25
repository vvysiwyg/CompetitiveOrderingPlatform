using MediatR;
using OrdersShared.Features.Orders;
using OrdersShared.POCOs;
using System.Net.Http.Json;

namespace OrdersWebApp.Features.Orders.Handlers
{
    public class GetCouriersHandler : IRequestHandler<GetCouriersRequest, GetCouriersRequest.Response>
    {
        private readonly HttpClient _httpClient;

        public GetCouriersHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetCouriersRequest.Response> Handle(GetCouriersRequest request, CancellationToken cancellationToken)
        {
            List<Courier>? response = await _httpClient.GetFromJsonAsync<List<Courier>?>(GetCouriersRequest.RouteTemplate);

            if (response is null)
                throw new Exception("Ошибка при вызове метода GetOrders");

            return new GetCouriersRequest.Response(response);
        }
    }
}
