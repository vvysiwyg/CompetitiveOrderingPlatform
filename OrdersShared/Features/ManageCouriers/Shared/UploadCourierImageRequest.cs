using MediatR;
using Microsoft.AspNetCore.Components.Forms;

namespace OrdersShared.Features.ManageCouriers.Shared
{
    public record UploadCourierImageRequest(int id, IBrowserFile file) : IRequest<UploadCourierImageRequest.Response>
    {
        public const string RouteTemplate = "/api/couriers/{id}/images";

        public record Response(string imageName);
    }
}
