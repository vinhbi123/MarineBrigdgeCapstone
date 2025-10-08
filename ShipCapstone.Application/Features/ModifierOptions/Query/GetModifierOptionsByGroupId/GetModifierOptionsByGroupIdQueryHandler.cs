using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.ModifierOptions;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ModifierOptions.Query.GetModifierOptionsByGroupId;

public class GetModifierOptionsByGroupIdQueryHandler : IRequestHandler<GetModifierOptionsByGroupIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public GetModifierOptionsByGroupIdQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(GetModifierOptionsByGroupIdQuery request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }

        var modifierGroup = await _unitOfWork.GetRepository<ModifierGroup>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ModifierGroupId && x.Supplier.AccountId == accountId,
            include: x => x.Include(x => x.ModifierOptions)
        );

        if (modifierGroup == null)
        {
            throw new NotFoundException("Không tìm thấy nhóm tùy chọn");
        }
        
        var response = modifierGroup.ModifierOptions.Select(x => new GetModifierOptionsByGroupIdResponse(
            x.Id,
            x.Name,
            x.DisplayOrder)
        ).OrderBy(x => x.DisplayOrder).ToList();
        
        return new ApiResponse
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy danh sách tùy chọn thành công",
            Data = response
        };
    }
}