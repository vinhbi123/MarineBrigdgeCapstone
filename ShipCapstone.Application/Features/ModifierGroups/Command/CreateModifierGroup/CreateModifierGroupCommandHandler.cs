using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ModifierGroups.Command.CreateModifierGroup;

public class CreateModifierGroupCommandHandler : IRequestHandler<CreateModifierGroupCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public CreateModifierGroupCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(CreateModifierGroupCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }

        var supplier = await _unitOfWork.GetRepository<Supplier>().SingleOrDefaultAsync(
            predicate: x => x.AccountId == accountId
        );
        if (supplier == null)
        {
            throw new NotFoundException("Nhà cung cấp không tồn tại");
        }

        var modifierGroup = new ModifierGroup()
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            SupplierId = supplier.Id,
            ModifierOptions = request.ModifierOptions.Select(x => new ModifierOption()
            {
                Id = Guid.CreateVersion7(),
                Name = x.Name,
                DisplayOrder = x.DisplayOrder
            }).ToList()
        };
        
        await _unitOfWork.GetRepository<ModifierGroup>().InsertAsync(modifierGroup);

        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            _logger.Error("Create modifier group failed");
            throw new Exception("Tạo nhóm tùy chọn thất bại");
        }
        _logger.Information("Create modifier group successfully");
        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Tạo nhóm tùy chọn thành công",
            Data = modifierGroup.Id
        };
    }
}