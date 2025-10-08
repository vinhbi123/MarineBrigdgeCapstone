using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Domain.Models.Authentication;

public class LoginResponse
{
    public Guid AccountId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string AccessToken { get; set; }
    public ERole Role { get; set; }
}