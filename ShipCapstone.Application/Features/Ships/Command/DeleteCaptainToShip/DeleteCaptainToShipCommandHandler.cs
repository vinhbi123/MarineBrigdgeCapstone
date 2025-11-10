using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Ships;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Ships.Command.DeleteCaptainToShip;

public class DeleteCaptainToShipCommandHandler : IRequestHandler<DeleteCaptainToShipCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly IClaimService _claimService;

    public DeleteCaptainToShipCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }

    public async ValueTask<ApiResponse> Handle(DeleteCaptainToShipCommand request, CancellationToken cancellationToken)
    {
        var userId = _claimService.GetCurrentUserId;
        var ship = await _unitOfWork.GetRepository<Ship>().SingleOrDefaultAsync(
            predicate: s => s.Id.Equals(request.Id)) ?? throw new NotFoundException("Không tìm thấy tàu");
        if (ship.AccountId != userId)
        {
            throw new BadHttpRequestException("Tàu không thuộc quyền sở hữu của bạn");
        }
        
        ship.CaptainId = null;
        _unitOfWork.GetRepository<Ship>().UpdateAsync(ship);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Có lỗi xảy ra trong quá trình xóa ủy quyền thuyền trưởng");
        }
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Xóa ủy quyền thuyền trưởng cho tàu thành công",
            Data = new GetShipByIdResponse()
            {
                Id = ship.Id,
                Name = ship.Name,
                BuildYear = ship.BuildYear,
                ImoNumber = ship.ImoNumber,
                Longitude = ship.Longitude,
                Latitude = ship.Latitude,
                RegisterNo = ship.RegisterNo,
                CaptainId = ship.CaptainId,
                CreatedDate = ship.CreatedDate,
                LastModifiedDate = ship.LastModifiedDate,
            }
        };
    }
}