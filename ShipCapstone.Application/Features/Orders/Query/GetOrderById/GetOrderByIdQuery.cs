using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Orders.Query
{
    public class GetOrdersQuery : IRequest<ApiResponse>
    {
        public Guid? ShipId { get; set; }
        public string? Status { get; set; }    
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Name { get; set; }
    }

}
