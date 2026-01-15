using Mediator;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ProductImages.Command.RemoveProductImage;

public class RemoveProductImageCommandHandler : IRequestHandler<RemoveProductImageCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public RemoveProductImageCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(RemoveProductImageCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Không tìm thấy thông tin người dùng");
        }

        var productImage = await _unitOfWork.GetRepository<ProductImage>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ProductImageId 
                             && x.Product.Category.Supplier.AccountId == accountId
        );
        
        if (productImage == null)
        {
            throw new BadHttpRequestException("Hình ảnh sản phẩm không tồn tại hoặc bạn không có quyền xóa hình ảnh này");
        }
        
        _unitOfWork.GetRepository<ProductImage>().DeleteAsync(productImage);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Xóa hình ảnh sản phẩm thất bại");
        }
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Xóa hình ảnh sản phẩm thành công",
            Data = productImage.Id
        };
    }
}