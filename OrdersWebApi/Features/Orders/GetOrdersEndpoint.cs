using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersShared.Features.Orders;
using OrdersShared.POCOs;

namespace OrdersWebApi.Features.Orders
{
    public class GetOrdersEndpoint : EndpointBaseAsync
        .WithRequest<string>
        .WithResult<ActionResult<List<Order>?>>
    {
        private readonly OrdersContext _context;

        public GetOrdersEndpoint(OrdersContext context)
        {
            _context = context;
        }

        [HttpGet(GetOrdersRequest.RouteTemplate)]
        public override async Task<ActionResult<List<Order>?>> HandleAsync(string status, CancellationToken cancellationToken = default)
        {
            string newStatus;

            switch (status)
            {
                case "active-orders":
                    newStatus = "Выполняется";
                    break;
                case "archive":
                    newStatus = "Завершен";
                    break;
                case "assigned-orders":
                    newStatus = "Назначен";
                    break;
                default:
                    newStatus = "Не назначен";
                    break;
            }

            var result = _context.Orders.
                Select(s => new
                {
                    Id = s.Id,
                    Number = s.Number,
                    Sum = s.Sum,
                    CreationDate = s.CreationDate,
                    Status = s.Status,
                    Delivery = new
                    {
                        Id = s.Delivery.Id,
                        Address = new
                        {
                            Id = s.Delivery.Address.Id,
                            City = s.Delivery.Address.City,
                            StreetName = s.Delivery.Address.StreetName,
                            BuildingNum = s.Delivery.Address.BuildingNum,
                            ApartmentNum = s.Delivery.Address.ApartmentNum,
                            EntranceNum = s.Delivery.Address.EntranceNum
                        }
                    }
                })
                .Where(x => x.Status == newStatus)
                .AsNoTracking();

            return Ok(await result.ToListAsync());
        }
    }
}
