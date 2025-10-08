using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ModifierOptions.Command.CreateModifierOption;

public class CreateModifierOptionCommandHandler : IRequestHandler<CreateModifierOptionCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public CreateModifierOptionCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(CreateModifierOptionCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }

        var modifierGroup = await _unitOfWork.GetRepository<ModifierGroup>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ModifierGroupId 
                            && x.Supplier.AccountId == accountId
        );
        if (modifierGroup == null)
        {
            throw new NotFoundException("Nhóm tùy chọn không tồn tại");
        }
        
        var modifierOption = new ModifierOption()
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            DisplayOrder = request.DisplayOrder,
            ModifierGroupId = modifierGroup.Id
        };
        await _unitOfWork.GetRepository<ModifierOption>().InsertAsync(modifierOption);
        
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            _logger.Error("Create modifier option failed");
            throw new Exception("Tạo tùy chọn thất bại");
        }
        return new ApiResponse
        {
            Status = StatusCodes.Status201Created,
            Message = "Tùy chọn được tạo thành công",
            Data = modifierOption.Id
        };
    }
}