using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.ModifierGroups.Command.CreateModifierGroup;

public class CreateModifierGroupCommand : IRequest<ApiResponse>
{
    public string Name { get; set; }
    public List<CreateModifierOptionRequest> ModifierOptions { get; set; }
}

public class CreateModifierOptionRequest
{
    public string Name { get; set; }
    public int DisplayOrder { get; set; }
}