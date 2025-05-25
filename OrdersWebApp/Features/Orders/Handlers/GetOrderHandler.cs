using MediatR;
using OrdersShared.Features.Orders;
using OrdersShared.POCOs;
using System.Net.Http.Json;

namespace OrdersWebApp.Features.Orders.Handlers
{
    public class GetOrderHandler : IRequestHandler<GetOrderRequest, GetOrderRequest.Response>
    {
        private readonly HttpClient _httpClient;

        public GetOrderHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetOrderRequest.Response> Handle(GetOrderRequest request, CancellationToken cancellationToken)
        {
            Order? response = await _httpClient.GetFromJsonAsync<Order?>(request.GetFullRoute());

            if (response is null)
                throw new Exception("Ошибка при вызове метода GetOrders");

            return new GetOrderRequest.Response(response);
        }
    }
}
