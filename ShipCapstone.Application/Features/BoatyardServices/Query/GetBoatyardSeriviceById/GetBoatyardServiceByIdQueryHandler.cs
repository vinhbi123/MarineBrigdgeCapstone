using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.BoatyardServices;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.BoatyardServices.Query.GetBoatyardSeriviceById;

public class GetBoatyardServiceByIdQueryHandler : IRequestHandler<GetBoatyardServiceByIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public GetBoatyardServiceByIdQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger,
        IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(GetBoatyardServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }

        var boatyardService = await _unitOfWork.GetRepository<BoatyardService>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.BoatyardServiceId 
                            && x.Boatyard.AccountId == accountId
        );
        
        if(boatyardService == null)
        {
            throw new NotFoundException("Không tìm thấy dịch vụ xưởng");
        }
        
        var response = new GetBoatyardServiceByIdResponse()
        {
            Id = boatyardService.Id,
            TypeService = boatyardService.TypeService,
            Price = boatyardService.Price,
            IsActive = boatyardService.IsActive,
            CreatedDate = boatyardService.CreatedDate,
            LastModifiedDate = boatyardService.LastModifiedDate
        };
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy dịch vụ xưởng thành công",
            Data = response
        };
    }
}