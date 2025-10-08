using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Ships.Command.UpdateShip;

public class UpdateShipCommand : IRequest<ApiResponse>
{
    public Guid ShipId { get; set; }
    public string? Name { get; set; }
    public string? ImoNumber { get; set; }
    public string? RegisterNo { get; set; }
    public int? BuildYear { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
}

public class UpdateShipRequest
{
    public string? Name { get; set; }
    public string? ImoNumber { get; set; }
    public string? RegisterNo { get; set; }
    public int? BuildYear { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
}