using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Settings;
using ShipCapstone.Infrastructure.Utils;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ShipCapstone.Application.Services.Implements;

public class AuthenticationService : IAuthenticationService
{
    private readonly JwtSettings _jwtSettings;

    public AuthenticationService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
    public string GenerateAccessToken(Account account)
    {
        var tokenhandler = new JwtSecurityTokenHandler();
        var tokenkey = Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey!);
        var timeExpire = TimeUtil.GetCurrentSEATime().AddDays((double)_jwtSettings.TokenExpiry!);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new Claim("AccountId", account.Id.ToString()),
                    new Claim("Username", account.Username),
                    new Claim(ClaimTypes.Role, account.Role.ToString()),
                    new Claim(ClaimTypes.Email, account.Email)
                }
            ),
            Expires = timeExpire,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenhandler.CreateToken(tokenDescriptor);
        var tokenString = tokenhandler.WriteToken(token);
        return tokenString;
    }
}