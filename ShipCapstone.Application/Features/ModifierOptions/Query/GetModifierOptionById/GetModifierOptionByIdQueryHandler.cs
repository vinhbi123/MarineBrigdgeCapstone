using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.ModifierOptions;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ModifierOptions.Query.GetModifierOptionById;

public class GetModifierOptionByIdQueryHandler : IRequestHandler<GetModifierOptionByIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public GetModifierOptionByIdQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(GetModifierOptionByIdQuery request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }

        var modifierOption = await _unitOfWork.GetRepository<ModifierOption>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ModifierOptionId && x.ModifierGroup.Supplier.AccountId == accountId
        );

        if (modifierOption == null)
        {
            throw new NotFoundException("Không tìm thấy tùy chọn");
        }
        
        var response = new GetModifierOptionByIdResponse()
        {
            Id = modifierOption.Id,
            Name = modifierOption.Name,
            DisplayOrder = modifierOption.DisplayOrder,
        };
        
        return new ApiResponse
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy thông tin tùy chọn thành công",
            Data = response
        };
    }
}