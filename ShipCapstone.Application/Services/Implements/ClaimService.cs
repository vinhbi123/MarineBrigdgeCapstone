using System.Security.Claims;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Services.Interfaces;

namespace ShipCapstone.Application.Services.Implements;

public class ClaimService : IClaimService
{
    public ClaimService(IHttpContextAccessor httpContextAccessor)
    {
        var identity = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
        var accountId = Guid.TryParse(JwtUtil.GetCurrentAccountId(identity), out var accountIdResult ) ? accountIdResult : Guid.Empty;
        var username = JwtUtil.GetCurrentUsername(identity);
        var role = JwtUtil.GetRole(identity);
        var email = JwtUtil.GetCurrentEmail(identity);
        GetCurrentUserId = accountId == Guid.Empty ? Guid.Empty : accountId;
        GetCurrentUsername = string.IsNullOrEmpty(username) ? "" : username;
        GetRole = string.IsNullOrEmpty(role) ? string.Empty : role;
        GetCurrentEmail = string.IsNullOrEmpty(email) ? string.Empty : email;
    }
    
    public Guid GetCurrentUserId { get; }
    public string GetCurrentUsername { get; }
    public string GetRole { get; }
    public string GetCurrentEmail { get; }
}