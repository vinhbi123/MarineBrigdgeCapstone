using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Captains.Command.RegisterCaptain;

public class RegisterCaptainCommand : IRequest<ApiResponse>
{
    public string FullName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public IFormFile? Avatar { get; set; }
}