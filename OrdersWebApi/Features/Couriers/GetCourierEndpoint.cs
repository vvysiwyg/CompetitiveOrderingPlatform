using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersShared.Features.Orders;
using OrdersShared.POCOs;

namespace OrdersWebApi.Features.Orders
{
    public class GetCourierEndpoint : EndpointBaseAsync
        .WithRequest<int>
        .WithResult<ActionResult<Courier?>>
    {
        private readonly OrdersContext _context;

        public GetCourierEndpoint(OrdersContext context)
        {
            _context = context;
        }

        [HttpGet(GetCourierRequest.RouteTemplate)]
        public override async Task<ActionResult<Courier?>> HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = _context.Couriers.
            Include(c => c.Emp).
            Include(c => c.CourierHistories).
            Include(c => c.CourierOrders).
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
                    Email = s.Emp.Email,
                    PasswordHash = "",
                    Position = s.Emp.Position,
                    Photo = s.Emp.Photo,
                    MobilePhone = s.Emp.MobilePhone,
                    Couriers = Array.Empty<object>()
                },
                CourierHistories = s.CourierHistories.Select(s2 => new
                {
                    Id = s2.Id,
                    NumberOfSuccessOrders = s2.NumberOfSuccessOrders,
                    NumberOfFailedOrders = s2.NumberOfFailedOrders,
                    Income = s2.Income,
                    MeasurementDate = s2.MeasurementDate,
                    CourierId = s2.CourierId,
                    Courier = new { }
                }),
                CourierOrders = s.CourierOrders.Select(s2 => new
                {
                    Cost = s2.Cost,
                    OrderId = s2.OrderId,
                    CourierId = s2.CourierId,
                    Courier = new { },
                    Order = new
                    {
                        Id = s2.Order.Id,
                        Number = s2.Order.Number,
                        Sum = s2.Order.Sum,
                        CreationDate = s2.Order.CreationDate,
                        PaymentType = s2.Order.PaymentType,
                        Status = s2.Order.Status,
                        DeliveryId = s2.Order.DeliveryId,
                        CustomerId = s2.Order.CustomerId,
                        Delivery = new { },
                        Customer = new { },
                        OrderDetailProducts = Array.Empty<object>(),
                        OrderDetailServices = Array.Empty<object>(),
                        CourierOrders = Array.Empty<object>()
                    }
                })
            }).
            Where(w => w.Id == id).
            AsNoTracking();

            // Если что-то получили вызываем метод Ok, иначе NotFound
            return Ok(await result.FirstOrDefaultAsync());
        }
    }
}
