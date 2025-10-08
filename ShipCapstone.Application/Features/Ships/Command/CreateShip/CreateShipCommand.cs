using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Ships.Command.CreateShip;

public class CreateShipCommand : IRequest<ApiResponse>
{
    public string Name { get; set; }
    public string? ImoNumber { get; set; }
    public string? RegisterNo { get; set; }
    public int? BuildYear { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
}