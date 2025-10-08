using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.ModifierOptions.Query.GetModifierOptionById;

public class GetModifierOptionByIdQuery : IRequest<ApiResponse>
{
    public Guid ModifierOptionId { get; set; }
}