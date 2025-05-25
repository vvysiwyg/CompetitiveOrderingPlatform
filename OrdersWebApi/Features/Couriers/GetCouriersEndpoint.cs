using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersShared.Features.Orders;
using OrdersShared.POCOs;

namespace OrdersWebApi.Features.Orders
{
    public class GetCouriersEndpoint : EndpointBaseAsync
        .WithoutRequest
        .WithResult<ActionResult<List<Courier>?>>
    {
        private readonly OrdersContext _context;

        public GetCouriersEndpoint(OrdersContext context)
        {
            _context = context;
        }

        [HttpGet(GetCouriersRequest.RouteTemplate)]
        public override async Task<ActionResult<List<Courier>?>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var result = _context.Couriers.
            Include(c => c.Emp).
            Select(s => new
            {
                Id = s.Id,
                NumberOfSuccessOrders = s.NumberOfSuccessOrders,
                NumberOfFailedOrders = s.NumberOfFailedOrders,
                EmpId = s.EmpId,
                Emp = new
                {
                    Id = s.Emp.Id,
                    FirstName = s.Emp.FirstName,
                    MiddleName = s.Emp.MiddleName,
                    LastName = s.Emp.LastName,
                    Position = s.Emp.Position,
                    PasswordHash = "",
                    Photo = s.Emp.Photo,
                    MobilePhone = s.Emp.MobilePhone,
                    Email = s.Emp.Email,
                },
                CourierHistories = Array.Empty<object>(),
                CourierOrders = Array.Empty<object>()
            }).
            AsNoTracking();

            return Ok(await result.ToListAsync());
        }
    }
}
