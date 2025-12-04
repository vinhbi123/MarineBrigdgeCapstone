using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Accounts.Command.ChangePassword;

public class ChangePasswordCommand : IRequest<ApiResponse>
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}