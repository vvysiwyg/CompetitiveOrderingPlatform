using MediatR;
using OrdersShared.Features.Orders;
using OrdersShared.POCOs;
using System.Net.Http.Json;

namespace OrdersWebApp.Features.Orders.Handlers
{
    public class GetCourierHandler : IRequestHandler<GetCourierRequest, GetCourierRequest.Response>
    {
        private readonly HttpClient _httpClient;

        public GetCourierHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetCourierRequest.Response> Handle(GetCourierRequest request, CancellationToken cancellationToken)
        {
            Courier? response = await _httpClient.GetFromJsonAsync<Courier?>(request.GetFullRoute());

            if (response is null)
                throw new Exception("Ошибка при вызове метода GetOrders");

            return new GetCourierRequest.Response(response);
        }
    }
}
