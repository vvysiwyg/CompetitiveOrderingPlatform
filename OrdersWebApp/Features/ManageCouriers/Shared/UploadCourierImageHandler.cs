using MediatR;
using OrdersShared.Features.ManageCouriers.Shared;

namespace OrdersWebApp.Features.ManageCouriers.Shared
{
    public class UploadCourierImageHandler : IRequestHandler<UploadCourierImageRequest, UploadCourierImageRequest.Response>
    {
        private readonly HttpClient _httpClient;

        public UploadCourierImageHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UploadCourierImageRequest.Response> Handle(UploadCourierImageRequest request, CancellationToken cancellationToken)
        {
            var fileContent = request.file.OpenReadStream(request.file.Size, cancellationToken);

            using var content = new MultipartFormDataContent();
            content.Add(new StreamContent(fileContent), "img", request.file.Name);

            var response = await _httpClient.PostAsync(UploadCourierImageRequest.RouteTemplate.Replace("{id}", request.id.ToString()), content, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var fileName = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
                return new UploadCourierImageRequest.Response(fileName);
            }
            else
            {
                return new UploadCourierImageRequest.Response("");
            }
        }
    }
}
