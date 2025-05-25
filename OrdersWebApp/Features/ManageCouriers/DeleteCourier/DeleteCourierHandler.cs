using MediatR;
using OrdersShared.Features.ManageCouriers.DeleteCourier;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.Text.Json;
using OrdersShared.Features.ManageCouriers.EditCourier;

namespace OrdersWebApp.Features.ManageCouriers.DeleteCourier
{
    public class DeleteCourierHandler : IRequestHandler<DeleteCourierRequest, DeleteCourierRequest.Response>
    {
        private readonly HttpClient _httpClient;

        public DeleteCourierHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DeleteCourierRequest.Response> Handle(DeleteCourierRequest request, CancellationToken cancellationToken)
        {
            string jsonPayload = JsonSerializer.Serialize(request.ids);
            Console.WriteLine(jsonPayload);
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Delete, DeleteCourierRequest.RouteTemplate)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };

            Console.WriteLine("Sending request");
            HttpResponseMessage response = await _httpClient.SendAsync(httpRequest);
            Console.WriteLine("Got response");

            if (response.IsSuccessStatusCode)
            {
                return new DeleteCourierRequest.Response(true);
            }
            else
            {
                return new DeleteCourierRequest.Response(false);
            }
        }
    }
}
