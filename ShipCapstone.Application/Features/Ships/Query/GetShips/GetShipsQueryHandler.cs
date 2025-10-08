using Mediator;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Ships;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Ships.Query.GetShips;

public class GetShipsQueryHandler : IRequestHandler<GetShipsQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public GetShipsQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger,
        IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(GetShipsQuery request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Không tìm thấy tài khoản.");
        }

        var ships = await _unitOfWork.GetRepository<Ship>().GetPagingListAsync(
            selector: x => new GetShipsResponse()
            {
                Id = x.Id,
                Name = x.Name,
                ImoNumber = x.ImoNumber,
                RegisterNo = x.RegisterNo,
                BuildYear = x.BuildYear,
                Longitude = x.Longitude,
                Latitude = x.Latitude,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate
            },
            predicate: x => x.AccountId == accountId 
                            && (string.IsNullOrEmpty(request.Name) || x.Name.Contains(request.Name)),
            page: request.Page,
            size: request.Size,
            sortBy: request.SortBy ?? nameof(Ship.CreatedDate),
            isAsc: request.IsAsc
        );
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy danh sách tàu thành công",
            Data = ships
        };
        
    }
}