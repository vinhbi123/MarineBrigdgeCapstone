using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.ModifierOptions.Command.UpdateModifierOption;

public class UpdateModifierOptionCommand : IRequest<ApiResponse>
{
    public Guid ModifierOptionId { get; set; }
    public string? Name { get; set; }
    public int? DisplayOrder { get; set; }
}

public class UpdateModifierOptionRequest
{
    public string? Name { get; set; }
    public int? DisplayOrder { get; set; }
}