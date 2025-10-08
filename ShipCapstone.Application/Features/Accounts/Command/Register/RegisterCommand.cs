using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Accounts.Command.Register;

public class RegisterCommand : IRequest<ApiResponse>
{
    public string FullName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public IFormFile? Avatar { get; set; }
    public string Otp { get; set; } = null!;
}