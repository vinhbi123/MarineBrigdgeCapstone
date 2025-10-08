using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.ModifierOptions.Query.GetModifierOptionsByGroupId;

public class GetModifierOptionsByGroupIdQuery : IRequest<ApiResponse>
{
    public Guid ModifierGroupId { get; set; }
}