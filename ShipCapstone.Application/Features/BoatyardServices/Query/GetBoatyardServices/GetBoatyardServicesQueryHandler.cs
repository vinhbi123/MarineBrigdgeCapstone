using Mediator;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.BoatyardServices;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.BoatyardServices.Query.GetBoatyardServices;

public class GetBoatyardServicesQueryHandler : IRequestHandler<GetBoatyardServicesQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public GetBoatyardServicesQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger,
        IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(GetBoatyardServicesQuery request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }

        var boatyardServices = await _unitOfWork.GetRepository<BoatyardService>().GetPagingListAsync(
            selector: x => new GetBoatyardServicesResponse()
            {
                Id = x.Id,
                TypeService = x.TypeService,
                Price = x.Price,
                IsActive = x.IsActive,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate
            },
            predicate: x => x.Boatyard.AccountId == accountId 
                            && (string.IsNullOrEmpty(request.TypeService) || x.TypeService.Contains(request.TypeService)),
            page: request.Page,
            size: request.Size,
            sortBy: request.SortBy ?? nameof(BoatyardService.CreatedDate),
            isAsc: request.IsAsc
        );
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy danh sách dịch vụ xưởng thành công",
            Data = boatyardServices
        };
    }
}