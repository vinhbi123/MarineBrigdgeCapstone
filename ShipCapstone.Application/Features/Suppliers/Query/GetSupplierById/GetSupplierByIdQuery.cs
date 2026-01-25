using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Suppliers.Query.GetSupplierById;

public class GetSupplierByIdQuery : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
}