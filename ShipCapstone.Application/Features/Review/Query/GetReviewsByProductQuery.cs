using Mediator;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Reviews;
using ShipCapstone.Infrastructure.Paginate.Interface;

namespace ShipCapstone.Application.Features.Review.Query.GetReviewsByProduct;

public class GetReviewsByProductQuery : IRequest<ApiResponse<IPaginate<GetReviewResponse>>>
{
    public Guid ProductId { get; set; }
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 20;
    public string? SortBy { get; set; } = null;
    public bool IsAsc { get; set; } = false;
}
   