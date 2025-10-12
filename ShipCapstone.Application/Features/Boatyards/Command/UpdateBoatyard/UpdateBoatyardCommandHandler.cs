using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Boatyards.Command.UpdateBoatyard;

public class UpdateBoatyardCommandHandler : IRequestHandler<UpdateBoatyardCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    private readonly IUploadService _uploadService;

    public UpdateBoatyardCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger,
        IClaimService claimService, IUploadService uploadService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        _uploadService = uploadService ?? throw new ArgumentNullException(nameof(uploadService));
    }
    
    public async ValueTask<ApiResponse> Handle(UpdateBoatyardCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }

        var boatyard = await _unitOfWork.GetRepository<Boatyard>().SingleOrDefaultAsync(
            predicate: x => x.AccountId == accountId,
            include: x => x.Include(x => x.Account)
        );
        if (boatyard == null)
        {
            throw new NotFoundException("Không tìm thấy xưởng");
        }

        boatyard.Name = request.Name ?? boatyard.Name;
        boatyard.Longitude = request.Longitude ?? boatyard.Longitude;
        boatyard.Latitude = request.Latitude ?? boatyard.Latitude;
        boatyard.Account.FullName = request.FullName ?? boatyard.Account.FullName;
        boatyard.Account.Address = request.Address ?? boatyard.Account.Address;
        boatyard.Account.PasswordHash = request.Password != null 
            ? PasswordUtil.HashPassword(request.Password) 
            : boatyard.Account.PasswordHash;
        if (request.Avatar != null)
        {
            var avatarUrl = await _uploadService.UploadImageAsync(request.Avatar);
            boatyard.Account.AvatarUrl = avatarUrl;
        }
        
        _unitOfWork.GetRepository<Boatyard>().UpdateAsync(boatyard);

        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Cập nhật xưởng thất bại");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Cập nhật xưởng thành công",
            Data = boatyard.Id
        };
    }
}