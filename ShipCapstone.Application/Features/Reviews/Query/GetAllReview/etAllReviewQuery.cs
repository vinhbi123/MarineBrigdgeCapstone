using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Reviews.Query.GetAllReview;

public class GetAllReviewQuery : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
    public string? SortBy { get; set; }
    public bool IsAsc { get; set; }
}