using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Ships.Command.CreateShip;

public class CreateShipCommandHandler : IRequestHandler<CreateShipCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public CreateShipCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger,
        IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(CreateShipCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Không tìm thấy tài khoản.");
        }

        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: x => x.Id == accountId && x.Role == ERole.User
        );
        
        if (account == null)
        {
            throw new NotFoundException("Không tìm thấy tài khoản.");
        }

        var ship = new Ship()
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            ImoNumber = request.ImoNumber,
            RegisterNo = request.RegisterNo,
            BuildYear = request.BuildYear,
            Longitude = request.Longitude,
            Latitude = request.Latitude,
            AccountId = account.Id
        };
        
        await _unitOfWork.GetRepository<Ship>().InsertAsync(ship);

        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            _logger.Error("Lỗi khi tạo tàu");
            throw new Exception("Lỗi khi tạo tàu");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Tạo tàu thành công",
            Data = ship.Id
        };
    }
}