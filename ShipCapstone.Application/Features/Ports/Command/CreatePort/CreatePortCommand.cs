using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Ports.Command.CreatePort;

public class CreatePortCommand : IRequest<ApiResponse>
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string? City { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
}