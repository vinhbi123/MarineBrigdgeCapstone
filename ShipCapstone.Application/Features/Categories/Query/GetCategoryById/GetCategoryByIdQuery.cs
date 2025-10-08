using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Categories.Query.GetCategoryById;

public class GetCategoryByIdQuery : IRequest<ApiResponse>
{
    public Guid CategoryId { get; set; }
}