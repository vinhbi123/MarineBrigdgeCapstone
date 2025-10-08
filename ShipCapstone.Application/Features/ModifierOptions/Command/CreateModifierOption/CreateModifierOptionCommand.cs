using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.ModifierOptions.Command.CreateModifierOption;

public class CreateModifierOptionCommand : IRequest<ApiResponse>
{
    public Guid ModifierGroupId { get; set; }
    public string Name { get; set; }
    public int DisplayOrder { get; set; }
}

public class CreateModifierOptionRequest
{
    public string Name { get; set; }
    public int DisplayOrder { get; set; }
}