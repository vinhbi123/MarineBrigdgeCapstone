using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ProductVariantOptions.Command.RemoveProductVariantOption;

public class RemoveProductVariantOptionCommandHandler : IRequestHandler<RemoveProductVariantOptionCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public RemoveProductVariantOptionCommandHandler(
        IUnitOfWork<ShipCapstoneContext> unitOfWork,
        ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(RemoveProductVariantOptionCommand request, CancellationToken cancellationToken)
    {
        var userId = _claimService.GetCurrentUserId;
        if (userId == Guid.Empty)
        {
            throw new BadHttpRequestException("Không tìm thấy thông tin người dùng");
        }
        
        var supplier = await _unitOfWork.GetRepository<Supplier>().SingleOrDefaultAsync(
            predicate: x => x.AccountId == userId
        );
        if (supplier == null)
        {
            throw new BadHttpRequestException("Nhà cung cấp không tồn tại");
        }
        
        var productVariantOption = await _unitOfWork.GetRepository<ProductVariantOption>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ProductVariantOptionId && x.ProductVariant.Product.SupplierId == supplier.Id
        );
        
        if (productVariantOption == null)
        {
            throw new NotFoundException("Tùy chọn biến thể sản phẩm không tồn tại hoặc bạn không có quyền xóa nó");
        }
        
        _unitOfWork.GetRepository<ProductVariantOption>().DeleteAsync(productVariantOption);
        
        var isSuccess = await _unitOfWork.CommitAsync() > 0;

        if (!isSuccess)
        {
            throw new Exception("Xóa tùy chọn biến thể sản phẩm thất bại");
        }
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Xóa tùy chọn biến thể sản phẩm thành công"
        };
    }
}