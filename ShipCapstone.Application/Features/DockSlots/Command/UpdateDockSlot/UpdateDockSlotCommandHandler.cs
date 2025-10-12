using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.DockSlots.Command.UpdateDockSlot;

public class UpdateDockSlotCommandHandler : IRequestHandler<UpdateDockSlotCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public UpdateDockSlotCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger,
        IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(UpdateDockSlotCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }
        
        var dockSlot = await _unitOfWork.GetRepository<DockSlot>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.DockSlotId && x.Boatyard.AccountId == accountId
        );
        
        if (dockSlot == null)
        {
            throw new NotFoundException("Chỗ neo đậu không tồn tại");
        }

        dockSlot.Name = request.Name;
        dockSlot.AssignedFrom = request.AssignedFrom;
        dockSlot.AssignedUntil = request.AssignedUntil;
        dockSlot.IsActive = request.IsActive;
        
        _unitOfWork.GetRepository<DockSlot>().UpdateAsync(dockSlot);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Cập nhật chỗ neo đậu thất bại");
        }
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Cập nhật chỗ neo đậu thành công",
            Data = dockSlot.Id
        };
    }
}