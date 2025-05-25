using MediatR;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using Ardalis.ApiEndpoints;
using OrdersShared.Features.ManageCouriers.Shared;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;

namespace OrdersWebApi.Features.ManageCouriers.Shared
{
    public class UploadCourierImageEndpoint : EndpointBaseAsync.
        WithRequest<int>.
        WithResult<ActionResult<string>>
    {
        private readonly OrdersContext _context;

        public UploadCourierImageEndpoint(OrdersContext context)
        {
            _context = context;
        }

        [HttpPost(UploadCourierImageRequest.RouteTemplate)]
        public override async Task<ActionResult<string>> HandleAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var courier = await _context.Couriers.Include(i => i.Emp).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (courier is null)
            {
                return BadRequest("Курьер не найден");
            }

            var file = Request.Form.Files[0];
            if (file.Length == 0)
            {
                return BadRequest("Фотография сотрудника не найдена");
            }

            var filename = $"{Guid.NewGuid()}.jpg";
            string baseDir = @"D:\VisualStudioProjects\OrdersWebApp\wwwroot";
            var saveLocation = Path.Combine(baseDir, "Imgs", filename);

            var resizeOptions = new ResizeOptions
            {
                Mode = ResizeMode.Pad,
                Size = new Size(640, 426)
            };

            using var image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream());
            image.Mutate(x => x.Resize(resizeOptions));
            await image.SaveAsJpegAsync(saveLocation, cancellationToken: cancellationToken);

            if (!string.IsNullOrWhiteSpace(courier.Emp.Photo))
            {
                System.IO.File.Delete(Path.Combine(baseDir, "Imgs", courier.Emp.Photo));
            }

            courier.Emp.Photo = $"/Imgs/{filename}";
            await _context.SaveChangesAsync(cancellationToken);

            return Ok(courier.Emp.Photo);
        }
    }
}
