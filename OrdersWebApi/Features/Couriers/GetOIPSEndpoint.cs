using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersShared.Features.Couriers;
using OrdersShared.POCOs;

namespace OrdersWebApi.Features.Couriers
{
    public class GetOIPSEndpoint : EndpointBaseAsync
        .WithoutRequest
        .WithResult<ActionResult<List<OrderIssuePoint>?>>
    {
        private readonly OrdersContext _context;

        public GetOIPSEndpoint(OrdersContext context)
        {
            _context = context;
        }

        [HttpGet(GetOIPSRequest.RouteTemplate)]
        public override async Task<ActionResult<List<OrderIssuePoint>?>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return Ok(await _context.OrderIssuePoints.AsNoTracking().ToListAsync());
        }
    }
}
