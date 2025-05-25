using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersShared.Features.Orders;
using OrdersShared.POCOs;

namespace OrdersWebApi.Features.Orders
{
    public class UnlinkCourierWithOrderEndpoint : EndpointBaseAsync
        .WithRequest<UnlinkCourierWithOrderRequest>
        .WithResult<ActionResult<bool>>
    {
        private readonly OrdersContext _context;

        public UnlinkCourierWithOrderEndpoint(OrdersContext context)
        {
            _context = context;
        }

        [HttpDelete(UnlinkCourierWithOrderRequest.RouteTemplate)]
        public override async Task<ActionResult<bool>> HandleAsync([FromRoute] UnlinkCourierWithOrderRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                CourierOrder? newCourierOrder = _context.CourierOrders.FirstOrDefault(co => co.CourierId == int.Parse(request.courierId) && co.OrderId == int.Parse(request.orderId));
                if (newCourierOrder != null)
                {
                    _context.CourierOrders.Remove(newCourierOrder);
                    await _context.SaveChangesAsync();
                    return Ok(true);
                }

                return Ok(false);
            }
            catch
            {
                return Ok(false);
            }
        }
    }
}
