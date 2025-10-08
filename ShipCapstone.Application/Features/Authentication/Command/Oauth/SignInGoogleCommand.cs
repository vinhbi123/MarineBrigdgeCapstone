using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Authentication.Command.Oauth;

public class SignInGoogleCommand : IRequest<ApiResponse>
{
    public string Code { get; set; }
    public bool IsAndroid { get; set; }
}