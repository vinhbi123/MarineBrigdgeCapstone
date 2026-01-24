using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.DockSlots.Query.GetDockSlotByBoatyardId;

public class GetDockSlotByBoatyardIdQuery : IRequest<ApiResponse>
{
    public Guid BoatyardId { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
    public string? SortBy { get; set; }
    public bool IsAsc { get; set; }
    public bool IsNoBooking { get; set; }
}