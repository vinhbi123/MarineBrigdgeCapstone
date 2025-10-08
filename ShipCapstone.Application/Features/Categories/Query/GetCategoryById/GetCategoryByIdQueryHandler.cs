using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Categories;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Categories.Query.GetCategoryById;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public GetCategoryByIdQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }

        var category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.CategoryId 
                            && x.Supplier.AccountId == accountId
        );
        if (category == null)
        {
            throw new NotFoundException("Danh mục không tồn tại");
        }

        var response = new GetCategoryByIdResponse()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ImageUrl = category.ImageUrl,
            IsActive = category.IsActive,
            CreatedDate = category.CreatedDate,
            LastModifiedDate = category.LastModifiedDate
        };

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy thông tin danh mục thành công",
            Data = response
        };
    }
}