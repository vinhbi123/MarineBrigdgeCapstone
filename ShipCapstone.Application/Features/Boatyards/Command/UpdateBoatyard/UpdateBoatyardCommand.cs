using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Boatyards.Command.UpdateBoatyard;

public class UpdateBoatyardCommand : IRequest<ApiResponse>
{
    public string? Name { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
    public string? FullName { get; set; }
    public string? Password { get; set; }
    public string? Address { get; set; }
    public IFormFile? Avatar { get; set; }
}