using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.DockSlots;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.DockSlots.Query.GetDockSlots;

public class GetDockSlotsQueryHandler : IRequestHandler<GetDockSlotsQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public GetDockSlotsQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(GetDockSlotsQuery request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new NotFoundException("Tài khoản không hợp lệ");
        }
        
        var dockSlots = await _unitOfWork.GetRepository<DockSlot>().GetPagingListAsync(
            selector: x => new GetDockSlotsResponse()
            {
                Id = x.Id,
                Name = x.Name,
                AssignedFrom = x.AssignedFrom,
                AssignedUntil = x.AssignedUntil,
                IsActive = x.IsActive
            },
            predicate: x => x.Boatyard.AccountId == accountId 
                            && (string.IsNullOrEmpty(request.Name) || x.Name.Contains(request.Name)),
            page: request.Page,
            size: request.Size,
            sortBy: request.SortBy ?? nameof(DockSlot.AssignedFrom),
            isAsc: request.IsAsc
        );
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy danh sách chỗ neo đậu thành công",
            Data = dockSlots
        };
    }
}