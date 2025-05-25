using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersShared.Features.ManageCouriers.DeleteCourier;
using OrdersShared.Features.ManageCouriers.EditCourier;
using OrdersShared.POCOs;

namespace OrdersWebApi.Features.ManageCouriers.DeleteCourier
{
    public class DeleteCourierEndpoint : EndpointBaseAsync.
        WithRequest<List<int>>.
        WithResult<ActionResult<bool>>
    {
        private readonly OrdersContext _context;

        public DeleteCourierEndpoint(OrdersContext context)
        {
            _context = context;
        }

        [HttpDelete(DeleteCourierRequest.RouteTemplate)]
        public override async Task<ActionResult<bool>> HandleAsync(List<int> request, CancellationToken cancellationToken = default)
        {
            try
            {
                List<Emp> emps = new List<Emp>();

                foreach (var courier in _context.Couriers.Include(i => i.Emp))
                {
                    if (request.Contains(courier.Id))
                        emps.Add(courier.Emp!);
                }

                _context.Emps.RemoveRange(emps);
                await _context.SaveChangesAsync(cancellationToken);

                return Ok(true);
            }
            catch
            {
                return Ok(false);
            }
        }
    }
}
