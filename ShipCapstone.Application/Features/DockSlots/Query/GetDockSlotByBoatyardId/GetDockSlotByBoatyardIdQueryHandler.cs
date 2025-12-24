using Mediator;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.DockSlots;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;
using ShipCapstone.Infrastructure.Utils;

namespace ShipCapstone.Application.Features.DockSlots.Query.GetDockSlotByBoatyardId;

public class GetDockSlotByBoatyardIdQueryHandler : IRequestHandler<GetDockSlotByBoatyardIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly  ILogger _logger;
    
    public GetDockSlotByBoatyardIdQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork,
        ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async ValueTask<ApiResponse> Handle(GetDockSlotByBoatyardIdQuery request, CancellationToken cancellationToken)
    {
        var time = TimeUtil.GetCurrentSEATime();
        _logger.Information($"BEGIN: {nameof(GetDockSlotByBoatyardIdQueryHandler)} - {time}");
        var dockSlots = await _unitOfWork.GetRepository<DockSlot>().GetPagingListAsync(
            selector: x => new GetDockSlotByBoatyardIdResponse()
            {
                Id = x.Id,
                Name = x.Name,
                AssignedFrom = x.AssignedFrom,
                AssignedUntil = x.AssignedUntil
            },
            predicate: x => x.IsActive && x.AssignedFrom <= TimeUtil.GetCurrentSEATime()
                                       && (x.AssignedUntil == null || x.AssignedUntil >= TimeUtil.GetCurrentSEATime())
                                       && x.BoatyardId.Equals(request.BoatyardId),
            page: request.Page,
            size: request.Size,
            sortBy: request.SortBy ?? nameof(DockSlot.Name),
            isAsc: request.IsAsc
        );

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy danh sách chỗ đậu thuyền thành công",
            Data = dockSlots
        };

    }
}