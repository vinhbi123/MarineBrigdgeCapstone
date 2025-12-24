using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ProductVariants.Command.UpdateProductVariant;

public class UpdateProductVariantCommandHandler : IRequestHandler<UpdateProductVariantCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public UpdateProductVariantCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(UpdateProductVariantCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }

        var supplier = await _unitOfWork.GetRepository<Supplier>().SingleOrDefaultAsync(
            predicate: x => x.AccountId == accountId
        );
        if (supplier == null)
        {
            throw new NotFoundException("Nhà cung cấp không tồn tại.");
        }
        
        var productVariant = await _unitOfWork.GetRepository<ProductVariant>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ProductVariantId && x.Product.SupplierId == supplier.Id
        );

        if (productVariant == null)
        {
            throw new NotFoundException("Biến thể sản phẩm không tồn tại hoặc bạn không có quyền chỉnh sửa biến thể sản phẩm này.");
        }
        
        productVariant.Name = request.Name ?? productVariant.Name;
        productVariant.Price = request.Price ?? productVariant.Price;

        if (request.IsActive != null)
        {
            var productId = productVariant.ProductId;
            var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
                predicate: x => x.Id == productId,
                include: x => x.Include(p => p.ProductVariants)
            );
            
            var activeVariantsCount = product.ProductVariants.Count(pv => pv.IsActive);
            if (activeVariantsCount <= 1 && productVariant.IsActive && request.IsActive == false)
            {
                throw new BadHttpRequestException("Không thể hủy kích hoạt biến thể sản phẩm cuối cùng của sản phẩm.");
            }

            productVariant.IsActive = request.IsActive.Value;
        }
        
        _unitOfWork.GetRepository<ProductVariant>().UpdateAsync(productVariant);
        
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Cập nhật biến thể sản phẩm thất bại.");
        }
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Cập nhật biến thể sản phẩm thành công.",
            Data = productVariant.Id
        };
    }
}