using Mediator;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Products;

namespace ShipCapstone.Application.Features.Products.Query.GetProductById;

public class GetProductByIdQuery : IRequest<ApiResponse<GetProductResponse>>
{
    public Guid ProductId { get; set; }
}
