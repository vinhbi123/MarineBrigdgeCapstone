using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Models.Boatyards;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Boatyards.Query.GetBoatyardDetail;

public class GetBoatyardDetailQueryHandler : IRequestHandler<GetBoatyardDetailQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public GetBoatyardDetailQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(GetBoatyardDetailQuery request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }
        
        var boatyard = await _unitOfWork.GetRepository<Domain.Entities.Boatyard>().SingleOrDefaultAsync(
            predicate: x => x.AccountId == accountId,
            include: x => x.Include(x => x.Account)
        );
        if (boatyard == null)
        {
            throw new NotFoundException("Xưởng không tồn tại");
        }

        var response = new GetBoatyardDetailResponse()
        {
            Id = boatyard.Id,
            Name = boatyard.Name,
            Latitude = boatyard.Latitude,
            Longitude = boatyard.Longitude,
            AccountId = boatyard.AccountId,
            FullName = boatyard.Account.FullName,
            Username = boatyard.Account.Username,
            Address = boatyard.Account.Address,
            Email = boatyard.Account.Email,
            PhoneNumber = boatyard.Account.PhoneNumber,
            AvatarUrl = boatyard.Account.AvatarUrl,
            CreatedDate = boatyard.CreatedDate,
            LastModifiedDate = boatyard.LastModifiedDate
        };
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy thông tin xưởng thành công",
            Data = response
        };
    }
}