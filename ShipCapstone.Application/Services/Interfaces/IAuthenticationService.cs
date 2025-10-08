using ShipCapstone.Domain.Entities;

namespace ShipCapstone.Application.Services.Interfaces;

public interface IAuthenticationService
{
    string GenerateAccessToken(Account account);
}