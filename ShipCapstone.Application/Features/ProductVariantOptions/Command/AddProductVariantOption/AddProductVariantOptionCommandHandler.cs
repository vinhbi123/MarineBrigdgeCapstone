using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ProductVariantOptions.Command.AddProductVariantOption;

public class AddProductVariantOptionCommandHandler : IRequestHandler<AddProductVariantOptionCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public AddProductVariantOptionCommandHandler(
        IUnitOfWork<ShipCapstoneContext> unitOfWork,
        ILogger logger,
        IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    
    public async ValueTask<ApiResponse> Handle(AddProductVariantOptionCommand request, CancellationToken cancellationToken)
    {
        var userId = _claimService.GetCurrentUserId;
        if (userId == Guid.Empty) 
        {
            throw new BadHttpRequestException("Không tìm thấy thông tin người dùng");
        }
        
        var supplier = await _unitOfWork.GetRepository<Domain.Entities.Supplier>().SingleOrDefaultAsync(
            predicate: x => x.AccountId == userId
        );
        if (supplier == null)
        {
            throw new BadHttpRequestException("Nhà cung cấp không tồn tại");
        }

        var productVariant = await _unitOfWork.GetRepository<ProductVariant>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ProductVariantId && x.Product.SupplierId == supplier.Id
        );
        if (productVariant == null)
        {
            throw new NotFoundException("Biến thể sản phẩm không tồn tại hoặc bạn không có quyền thêm tùy chọn vào nó");
        }
        
        var modifierOption = await _unitOfWork.GetRepository<ModifierOption>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ModifierOptionId && x.ModifierGroup.SupplierId == supplier.Id
        );
        
        if (modifierOption == null)
        {
            throw new NotFoundException("Tùy chọn điều chỉnh không tồn tại hoặc bạn không có quyền sử dụng nó");
        }

        var productVariantOption = await _unitOfWork.GetRepository<ProductVariantOption>().SingleOrDefaultAsync(
            predicate: x => x.ProductVariantId == productVariant.Id && x.ModifierOptionId == modifierOption.Id
        );
        
        if (productVariantOption != null)
        {
            throw new BadHttpRequestException("Tùy chọn điều chỉnh đã được thêm vào biến thể sản phẩm này");
        }
        
        productVariantOption = new ProductVariantOption()
        {
            Id = Guid.CreateVersion7(),
            ProductVariantId = productVariant.Id,
            ModifierOptionId = modifierOption.Id
        };

        await _unitOfWork.GetRepository<ProductVariantOption>().InsertAsync(productVariantOption);
        
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Thêm tùy chọn biến thể sản phẩm thất bại");
        }
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Thêm tùy chọn biến thể sản phẩm thành công",
            Data = productVariantOption.Id
        };
    }
}