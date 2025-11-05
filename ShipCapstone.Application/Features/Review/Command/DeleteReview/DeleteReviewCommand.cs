using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Review.Command.DeleteReview;

public class DeleteReviewCommand : IRequest<ApiResponse>
{
    public Guid ReviewId { get; set; }
}
