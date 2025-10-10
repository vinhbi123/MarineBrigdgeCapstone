using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Boatyards.Command.CreateBoatyard;

public class CreateBoatyardCommand : IRequest<ApiResponse>
{
    public string Name { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public IFormFile? Avatar { get; set; }
    public string Otp { get; set; }
    public List<CreateDockSlotForCreateBoatyard> DockSlots { get; set; } = new();
}
public class CreateDockSlotForCreateBoatyard
{
    public string Name { get; set; }
    public DateTime AssignedFrom { get; set; }
    public DateTime? AssignedUntil { get; set; }
}