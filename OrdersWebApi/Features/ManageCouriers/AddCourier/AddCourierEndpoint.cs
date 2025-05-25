using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using OrdersShared.Features.ManageCouriers.AddCourier;
using OrdersShared.POCOs;

namespace OrdersWebApi.Features.ManageCouriers.AddCourier
{
    public class AddCourierEndpoint : EndpointBaseAsync.WithRequest<AddCourierRequest>.WithResult<ActionResult<int>>
    {
        private readonly OrdersContext _context;

        public AddCourierEndpoint(OrdersContext context)
        {
            _context = context;
        }

        [HttpPost(AddCourierRequest.RouteTemplate)]
        public override async Task<ActionResult<int>> HandleAsync(AddCourierRequest request, CancellationToken cancellationToken = default)
        {
            var newEmp = new Emp()
            {
                FirstName = request.courier.FirstName,
                LastName = request.courier.LastName,
                MiddleName = request.courier.MiddleName,
                Position = "Курьер",
                Email = request.courier.Email,
                MobilePhone = request.courier.MobilePhone
            };

            await _context.Emps.AddAsync(newEmp, cancellationToken);

            var newCourier = new Courier()
            {
                Emp = newEmp,
                NumberOfFailedOrders = 0,
                NumberOfSuccessOrders = 0
            };

            await _context.Couriers.AddAsync(newCourier, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Ok(newCourier.Id);
        }
    }
}
