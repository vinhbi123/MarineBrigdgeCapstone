using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Ships.Command.AssignCaptainToShip;

public class AssignCaptainToShipCommand : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
}

public class AssignCaptainToShipRequest
{
    public string? Email { get; set; }
}