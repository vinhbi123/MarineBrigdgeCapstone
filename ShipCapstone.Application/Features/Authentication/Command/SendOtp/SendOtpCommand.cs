using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Authentication.Command.SendOtp;

public class SendOtpCommand : IRequest<ApiResponse>
{
    public string Email { get; set; } = null!;
}