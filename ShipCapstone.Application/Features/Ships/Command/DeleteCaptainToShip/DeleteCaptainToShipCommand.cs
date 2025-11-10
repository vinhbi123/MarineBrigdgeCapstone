using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Ships.Command.DeleteCaptainToShip;

public class DeleteCaptainToShipCommand : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
}