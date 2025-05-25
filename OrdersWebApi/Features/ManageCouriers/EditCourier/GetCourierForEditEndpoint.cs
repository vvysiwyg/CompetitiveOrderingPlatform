using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersShared.Features.ManageCouriers.EditCourier;

namespace OrdersWebApi.Features.ManageCouriers.EditCourier
{
    public class GetCourierForEditEndpoint : EndpointBaseAsync.
        WithRequest<int>.
        WithResult<ActionResult<GetCourierForEditRequest.Response>>
    {
        private readonly OrdersContext _context;

        public GetCourierForEditEndpoint(OrdersContext context)
        {
            _context = context;
        }

        [HttpGet(GetCourierForEditRequest.RouteTemplate)]
        public override async Task<ActionResult<GetCourierForEditRequest.Response>> HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            var courier = await _context.Couriers.Include(x => x.Emp).FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

            if (courier is null)
            {
                return BadRequest("Не удалось найти курьера");
            }

            var response = new GetCourierForEditRequest.Response(new GetCourierForEditRequest.Courier(
                courier.Id,
                courier.Emp.FirstName,
                courier.Emp.LastName,
                courier.Emp.MiddleName,
                courier.Emp.Email,
                courier.Emp.MobilePhone,
                courier.Emp.Photo));

            return Ok(response);
        }
    }
}
