using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Products.Query.GetProductsBySupplierId;

public class GetProductsBySupplierIdQuery : IRequest<ApiResponse>
{
    public Guid SupplierId { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
    public string? SortBy { get; set; }
    public bool IsAsc { get; set; }
    public string? Name { get; set; }
}