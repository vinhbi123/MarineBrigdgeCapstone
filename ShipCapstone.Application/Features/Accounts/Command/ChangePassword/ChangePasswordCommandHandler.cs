using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Accounts.Command.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly IClaimService _claimService;

    public ChangePasswordCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    public async ValueTask<ApiResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: a => a.Id == accountId) ?? throw new NotFoundException("Không tìm thấy tài khoản");
        if (!account.PasswordHash.Equals(PasswordUtil.HashPassword(request.OldPassword)))
        {
            throw new BadHttpRequestException("Mật khẩu cũ không chính xác");
        }
        account.PasswordHash = PasswordUtil.HashPassword(request.NewPassword);
        _unitOfWork.GetRepository<Account>().UpdateAsync(account);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Có lỗi trong quá trình đổi mật khẩu");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Đổi mật khẩu thành công",
            Data = account.Id
        };
    }
}