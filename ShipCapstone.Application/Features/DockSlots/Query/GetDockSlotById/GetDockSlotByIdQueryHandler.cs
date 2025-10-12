using Mediator;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.DockSlots;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.DockSlots.Query.GetDockSlotById;

public class GetDockSlotByIdQueryHandler : IRequestHandler<GetDockSlotByIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public GetDockSlotByIdQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(GetDockSlotByIdQuery request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }

        var dockSlot = await _unitOfWork.GetRepository<DockSlot>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.DockSlotId && x.Boatyard.AccountId == accountId
        );

        var response = new GetDockSlotByIdResponse()
        {
            Id = dockSlot.Id,
            Name = dockSlot.Name,
            AssignedFrom = dockSlot.AssignedFrom,
            AssignedUntil = dockSlot.AssignedUntil,
            IsActive = dockSlot.IsActive
        };
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy thông tin chỗ neo đậu thành công",
            Data = response
        };
    }
}