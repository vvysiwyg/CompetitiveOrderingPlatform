using MediatR;
using OrdersShared.Features.Orders;
using System.Net.Http.Json;

namespace OrdersWebApp.Features.Orders.Handlers
{
    public class UnlinkCourierWithOrderHandler : IRequestHandler<UnlinkCourierWithOrderRequest, UnlinkCourierWithOrderRequest.Response>
    {
        private readonly HttpClient _httpClient;

        public UnlinkCourierWithOrderHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UnlinkCourierWithOrderRequest.Response> Handle(UnlinkCourierWithOrderRequest request, CancellationToken cancellationToken)
        {
            var result = await _httpClient.DeleteFromJsonAsync<bool>(request.GetFullRoute(), cancellationToken: cancellationToken);
            return new UnlinkCourierWithOrderRequest.Response(result);
        }
    }
}
