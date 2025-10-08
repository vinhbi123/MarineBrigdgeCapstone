using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ModifierOptions.Command.UpdateModifierOption;

public class UpdateModifierOptionCommandHandler : IRequestHandler<UpdateModifierOptionCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;

    public UpdateModifierOptionCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger,
        IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(UpdateModifierOptionCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;

        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }

        var modifierOption = await _unitOfWork.GetRepository<ModifierOption>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ModifierOptionId
            && x.ModifierGroup.Supplier.AccountId == accountId
        );
        
        if (modifierOption == null)
        {
            throw new NotFoundException("Tùy chọn không tồn tại");
        }
        
        modifierOption.Name = request.Name ?? modifierOption.Name;
        modifierOption.DisplayOrder = request.DisplayOrder ?? modifierOption.DisplayOrder;
        
        _unitOfWork.GetRepository<ModifierOption>().UpdateAsync(modifierOption);
        
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Cập nhật tùy chọn thất bại");
        }
        
        return new ApiResponse
        {
            Status = StatusCodes.Status200OK,
            Message = "Cập nhật tùy chọn thành công",
            Data = modifierOption.Id
        };
    }
}