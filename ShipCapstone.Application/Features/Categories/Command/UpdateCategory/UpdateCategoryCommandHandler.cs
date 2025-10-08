using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Categories.Command.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    private readonly IUploadService _uploadService;
    
    public UpdateCategoryCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger, IClaimService claimService,
        IUploadService uploadService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        _uploadService = uploadService ?? throw new ArgumentNullException(nameof(uploadService));
    }
    
    public async ValueTask<ApiResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }
        
        var category = await _unitOfWork.GetRepository<Domain.Entities.Category>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.CategoryId && x.Supplier.AccountId == accountId
        );
        
        if (category == null)
        {
            throw new NotFoundException("Danh mục không tồn tại");
        }
        
        category.Name = request.Name ?? category.Name;
        category.Description = request.Description ?? category.Description;
        category.IsActive = request.IsActive ?? category.IsActive;
        if (request.Image != null)
        {
            var imageUrl = await _uploadService.UploadImageAsync(request.Image);
            category.ImageUrl = imageUrl;
        }
        
        _unitOfWork.GetRepository<Domain.Entities.Category>().UpdateAsync(category);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Cập nhật danh mục thất bại");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Cập nhật danh mục thành công",
            Data = category.Id
        };
    }
}