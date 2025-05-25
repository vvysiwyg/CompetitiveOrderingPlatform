using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersShared.Features.Orders;
using OrdersShared.POCOs;

namespace OrdersWebApi.Features.Orders
{
    public class GetOrderEndpoint : EndpointBaseAsync
        .WithRequest<int>
        .WithResult<ActionResult<Order?>>
    {
        private readonly OrdersContext _context;

        public GetOrderEndpoint(OrdersContext context)
        {
            _context = context;
        }

        [HttpGet(GetOrderRequest.RouteTemplate)]
        public override async Task<ActionResult<Order?>> HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _context.Orders.
            Select(s => new
            {
                Id = s.Id,
                Number = s.Number,
                Sum = s.Sum,
                CreationDate = s.CreationDate,
                PaymentType = s.PaymentType,
                Status = s.Status,
                Delivery = new
                {
                    Id = s.Delivery.Id,
                    Date = s.Delivery.Date,
                    CallInHour = s.Delivery.CallInHour,
                    Commentary = s.Delivery.Commentary,
                    Address = new
                    {
                        Id = s.Delivery.Address.Id,
                        City = s.Delivery.Address.City,
                        StreetName = s.Delivery.Address.StreetName,
                        BuildingNum = s.Delivery.Address.BuildingNum,
                        ApartmentNum = s.Delivery.Address.ApartmentNum,
                        EntranceNum = s.Delivery.Address.EntranceNum
                    }
                },
                Customer = new
                {
                    Id = s.Customer.Id,
                    FirstName = s.Customer.FirstName,
                    MiddleName = s.Customer.MiddleName,
                    LastName = s.Customer.LastName,
                    Phone = s.Customer.Phone,
                    Email = s.Customer.Email,
                    CustomerType = s.Customer.CustomerType
                },
                OrderDetailProducts = s.OrderDetailProducts.Select(s2 => new
                {
                    Qty = s2.Qty,
                    Product = new
                    {
                        Id = s2.Product.Id,
                        Name = s2.Product.Name,
                        Price = s2.Product.Price
                    }
                }),
                OrderDetailServices = s.OrderDetailServices.Select(s2 => new
                {
                    Qty = s2.Qty,
                    Service = new
                    {
                        Id = s2.Service.Id,
                        Name = s2.Service.Name,
                        Price = s2.Service.Price
                    }
                })
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id);

            return Ok(result);
        }
    }
}
