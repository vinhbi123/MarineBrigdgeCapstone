using Mediator;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Ships;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Ships.Query.GetShipById;

public class GetShipByIdQueryHandler : IRequestHandler<GetShipByIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public GetShipByIdQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger,
        IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(GetShipByIdQuery request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Không tìm thấy tài khoản.");
        }

        var ship = await _unitOfWork.GetRepository<Ship>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ShipId && x.AccountId == accountId
        );
        
        if (ship == null)
        {
            throw new BadHttpRequestException("Không tìm thấy tàu.");
        }
        
        var response = new GetShipByIdResponse()
        {
            Id = ship.Id,
            Name = ship.Name,
            ImoNumber = ship.ImoNumber,
            RegisterNo = ship.RegisterNo,
            BuildYear = ship.BuildYear,
            Longitude = ship.Longitude,
            Latitude = ship.Latitude,
            CreatedDate = ship.CreatedDate,
            LastModifiedDate = ship.LastModifiedDate
        };

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy thông tin tàu thành công",
            Data = response
        };
    }
}