using MediatR;
using OrdersShared.Features.Orders;
using OrdersShared.POCOs;
using System.Net.Http.Json;

namespace OrdersWebApp.Features.Orders.Handlers
{
    public class GetOrdersHandler : IRequestHandler<GetOrdersRequest, GetOrdersRequest.Response>
    {
        private readonly HttpClient _httpClient;

        public GetOrdersHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetOrdersRequest.Response> Handle(GetOrdersRequest request, CancellationToken cancellationToken)
        {
            List<Order>? response = await _httpClient.GetFromJsonAsync<List<Order>?>(request.GetFullRoute());

            if (response is null)
                throw new Exception("Ошибка при вызове метода GetOrders");

            return new GetOrdersRequest.Response(response);
        }
    }
}
