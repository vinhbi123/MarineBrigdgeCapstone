using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Orders.Query
{
    public class GetAllOrdersQuery : IRequest<ApiResponse>
    {
        public Guid? ShipId { get; set; }        
        public string? Status { get; set; }     
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; }     
        public string? SortBy { get; set; }
        public bool IsAsc { get; set; } = false;
    }

}
