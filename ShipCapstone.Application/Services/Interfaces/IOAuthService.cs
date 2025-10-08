using ShipCapstone.Domain.Models.Authentication;

namespace ShipCapstone.Application.Services.Interfaces;

public interface IOAuthService
{
    Task<GoogleUserDto> GetUserByCode(string code, bool isAndroid = false);
}