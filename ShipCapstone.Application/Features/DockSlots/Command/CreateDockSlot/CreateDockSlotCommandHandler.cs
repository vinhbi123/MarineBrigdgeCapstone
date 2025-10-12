using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.DockSlots.Command.CreateDockSlot;

public class CreateDockSlotCommandHandler : IRequestHandler<CreateDockSlotCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public CreateDockSlotCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger,
        IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(CreateDockSlotCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;

        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }
        
        var boatyard = await _unitOfWork.GetRepository<Domain.Entities.Boatyard>().SingleOrDefaultAsync(
            predicate: x => x.AccountId == accountId
        );
        if (boatyard == null)
        {
            throw new NotFoundException("Không tìm thấy xưởng");
        }

        var dockSlot = new DockSlot()
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            AssignedFrom = request.AssignedFrom,
            AssignedUntil = request.AssignedUntil,
            BoatyardId = boatyard.Id,
            IsActive = true
        };

        await _unitOfWork.GetRepository<DockSlot>().InsertAsync(dockSlot);

        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Tạo chỗ neo đậu thất bại");
        }
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Tạo chỗ neo đậu thành công",
            Data = dockSlot.Id
        };
    }
}