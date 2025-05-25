using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersShared.Features.ManageCouriers.EditCourier;
using OrdersShared.Features.ManageCouriers.Shared;

namespace OrdersWebApi.Features.ManageCouriers.EditCourier
{
    public class EditCourierEndpoint : EndpointBaseAsync.
        WithRequest<EditCourierRequest>.
        WithResult<ActionResult<bool>>
    {
        private readonly OrdersContext _context;

        public EditCourierEndpoint(OrdersContext context)
        {
            _context = context;
        }

        [HttpPut(EditCourierRequest.RouteTemplate)]
        public override async Task<ActionResult<bool>> HandleAsync(EditCourierRequest request, CancellationToken cancellationToken = default)
        {
            var courier = await _context.Couriers.Include(x => x.Emp).FirstOrDefaultAsync(x => x.Id == request.courier.Id, cancellationToken: cancellationToken);

            if (courier is null)
            {
                return BadRequest("Не удалось найти курьера");
            }

            courier.Emp.FirstName = request.courier.FirstName;
            courier.Emp.LastName = request.courier.LastName;
            courier.Emp.FirstName = request.courier.FirstName;
            courier.Emp.Email = request.courier.Email;
            courier.Emp.MobilePhone = request.courier.MobilePhone;
            
            if (request.courier.ImageAction == ImageAction.Remove)
            {
                string baseDir = @"D:\VisualStudioProjects\OrdersWebApp\wwwroot";
                System.IO.File.Delete(baseDir + courier.Emp.Photo);
                courier.Emp.Photo = null;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Ok(true);
        }
    }
}
