using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ModifierGroups.Command.UpdateModifierGroup;

public class UpdateModifierGroupCommandHandler : IRequestHandler<UpdateModifierGroupCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public UpdateModifierGroupCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(UpdateModifierGroupCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }
        
        var modifierGroup = await _unitOfWork.GetRepository<Domain.Entities.ModifierGroup>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ModifierGroupId 
                            && x.Supplier.AccountId == accountId
        );
        if (modifierGroup == null)
        {
            throw new NotFoundException("Nhóm tùy chỉnh không tồn tại");
        }
        
        modifierGroup.Name = request.Name;
        
        _unitOfWork.GetRepository<ModifierGroup>().UpdateAsync(modifierGroup);
        
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Cập nhật nhóm tùy chỉnh thất bại");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Cập nhật nhóm tùy chỉnh thành công",
            Data = modifierGroup.Id
        };
    }
}