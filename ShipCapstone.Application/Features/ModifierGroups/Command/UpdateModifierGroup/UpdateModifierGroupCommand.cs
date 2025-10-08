using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.ModifierGroups.Command.UpdateModifierGroup;

public class UpdateModifierGroupCommand : IRequest<ApiResponse>
{
    public Guid ModifierGroupId { get; set; }
    public string Name { get; set; }
}
public class UpdateModifierGroupRequest
{
    public string Name { get; set; }
}