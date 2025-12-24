using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Features.Products.Command.UpdateProduct;
using ShipCapstone.Application.Services.Implements;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Profile;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Authentication.Command.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly IClaimService _claimService;
    private readonly IUploadService _uploadService;

    public UpdateProfileCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, IClaimService claimService, IUploadService uploadService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        _uploadService = uploadService ?? throw new ArgumentNullException(nameof(uploadService));
    }
    public async ValueTask<ApiResponse> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: a => a.Id == accountId) ?? throw new NotFoundException("Không tìm thấy tài khoản");
        account.FullName = request.FullName ?? account.FullName;
        account.Address = request.Address ?? account.Address;
        account.PhoneNumber = request.PhoneNumber ?? account.PhoneNumber;
        if (request.AvatarUrl != null)
        {
            account.AvatarUrl = await _uploadService.UploadImageAsync(request.AvatarUrl);
        }
        _unitOfWork.GetRepository<Account>().UpdateAsync(account);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Có một số lỗi trong quá trình cập nhật tài khoản");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Cập nhật tài khoản thành công",
            Data = new GetProfileResponse()
            {
                Id = account.Id,
                FullName = account.FullName,
                Address = account.Address,
                PhoneNumber = account.PhoneNumber,
                AvatarUrl = account.AvatarUrl,
            }
        };
    }
}