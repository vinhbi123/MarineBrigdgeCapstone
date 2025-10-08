using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Authentication.Command.Login;

public class LoginCommand : IRequest<ApiResponse>
{
    public string UsernameOrEmail { get; set; }
    public string Password { get; set; }
}