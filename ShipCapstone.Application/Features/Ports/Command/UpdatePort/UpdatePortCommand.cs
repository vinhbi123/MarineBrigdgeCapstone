using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Ports.Command.UpdatePort;

public class UpdatePortCommand : IRequest<ApiResponse>
{
    public Guid PortId { get; set; }
    public string? Name { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
}

public class UpdatePortRequest
{
    public string? Name { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
}