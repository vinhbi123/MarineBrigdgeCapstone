using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Orders.Query
{
    public class GetOrderByIdQuery : IRequest<ApiResponse>
    {
        public Guid Id { get; set; }
    }

}
