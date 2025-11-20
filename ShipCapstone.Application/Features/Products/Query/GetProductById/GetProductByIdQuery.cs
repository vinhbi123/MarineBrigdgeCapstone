using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Products.Query.GetProductById;

public class GetProductByIdQuery : IRequest<ApiResponse>
{
    public Guid ProductId { get; set; }
}