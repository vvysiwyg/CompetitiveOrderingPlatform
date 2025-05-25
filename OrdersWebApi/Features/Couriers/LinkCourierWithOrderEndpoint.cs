using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersShared.Features.Orders;
using OrdersShared.POCOs;

namespace OrdersWebApi.Features.Orders
{
    public class LinkCourierWithOrderEndpoint : EndpointBaseAsync
        .WithRequest<LinkCourierWithOrderRequest>
        .WithResult<ActionResult<bool>>
    {
        private readonly OrdersContext _context;

        public LinkCourierWithOrderEndpoint(OrdersContext context)
        {
            _context = context;
        }

        [HttpPost(LinkCourierWithOrderRequest.RouteTemplate)]
        public override async Task<ActionResult<bool>> HandleAsync(LinkCourierWithOrderRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                CourierOrder newCourierOrder = new CourierOrder
                {
                    CourierId = request.courierId,
                    OrderId = request.orderId,
                    Cost = request.deliveryCost
                };
                _context.CourierOrders.Add(newCourierOrder);
                await _context.SaveChangesAsync();

                return Ok(true);
            }
            catch
            {
                return Ok(false);
            }
        }
    }
}
