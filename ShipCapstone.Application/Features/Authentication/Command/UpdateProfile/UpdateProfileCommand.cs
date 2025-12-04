using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Authentication.Command.UpdateProfile;

public class UpdateProfileCommand : IRequest<ApiResponse>
{
    public string? FullName { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public IFormFile? AvatarUrl { get; set; }
}