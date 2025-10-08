using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Ships.Command.UpdateShip;

public class UpdateShipCommandHandler : IRequestHandler<UpdateShipCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public UpdateShipCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger,
        IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(UpdateShipCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Không tìm thấy tài khoản");
        }

        var ship = await _unitOfWork.GetRepository<Ship>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ShipId && x.AccountId == accountId
        );

        if (ship == null)
        {
            throw new NotFoundException("Không tìm thấy tàu");
        }
        
        ship.Name = request.Name ?? ship.Name;
        ship.ImoNumber = request.ImoNumber ?? ship.ImoNumber;
        ship.RegisterNo = request.RegisterNo ?? ship.RegisterNo;
        ship.BuildYear = request.BuildYear ?? ship.BuildYear;
        ship.Longitude = request.Longitude ?? ship.Longitude;
        ship.Latitude = request.Latitude ?? ship.Latitude;
        
        _unitOfWork.GetRepository<Ship>().UpdateAsync(ship);
        
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Lỗi khi cập nhật tàu");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Cập nhật tàu thành công",
            Data = ship.Id
        };
    }
}