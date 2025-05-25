using MediatR;
using OrdersShared.Features.Orders;
using System.Net.Http.Json;

namespace OrdersWebApp.Features.Orders.Handlers
{
    public class LinkCourierWithOrderHandler : IRequestHandler<LinkCourierWithOrderRequest, LinkCourierWithOrderRequest.Response>
    {
        private readonly HttpClient _httpClient;

        public LinkCourierWithOrderHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LinkCourierWithOrderRequest.Response> Handle(LinkCourierWithOrderRequest request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync(LinkCourierWithOrderRequest.RouteTemplate, request, cancellationToken);
            bool result = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancellationToken);
            return new LinkCourierWithOrderRequest.Response(result);
        }
    }
}
