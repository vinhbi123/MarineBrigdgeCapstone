using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.BoatyardServices.Query.GetBoatyardServicesByBoatyardId;

public class GetBoatyardServicesByBoatyardIdQuery : IRequest<ApiResponse>
{
    public Guid BoatyardId { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
    public string? SortBy { get; set; }
    public bool IsAsc { get; set; }
    public string? Name { get; set; }
}