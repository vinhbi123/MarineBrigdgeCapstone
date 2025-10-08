using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.ModifierGroups.Query.GetModifierGroupById;

public class GetModifierGroupByIdQuery : IRequest<ApiResponse>
{
    public Guid ModifierGroupId { get; set; }
}