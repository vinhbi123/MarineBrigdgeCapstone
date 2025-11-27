using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Orders.Query.GetOrderById
{
    public class GetAllOrderQuery : IRequest<ApiResponse>
    {
        public Guid Id { get; set; }
    }

}