using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.ProductImages.Command.CreateProductImage;

public class CreateProductImageCommand : IRequest<ApiResponse>
{
    public Guid ProductId { get; set; }
    public IFormFile Image { get; set; }
    public int? SortOrder { get; set; }
}