using Mediator;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ProductVariants.Command.CreateProductVariant;

public class CreateProductVariantCommandHandler : IRequestHandler<CreateProductVariantCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public CreateProductVariantCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(CreateProductVariantCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Không tìm thấy thông tin người dùng");
        }
        var supplier = await _unitOfWork.GetRepository<Supplier>().SingleOrDefaultAsync(
            predicate: x => x.AccountId == accountId
        );
        if (supplier == null)
        {
            throw new BadHttpRequestException("Nhà cung cấp không tồn tại");
        }
        
        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ProductId && x.SupplierId == supplier.Id
        );
        
        if (product == null)
        {
            throw new BadHttpRequestException("Sản phẩm không tồn tại hoặc bạn không có quyền thêm biến thể cho sản phẩm này");
        }
        
        var productVariant = new ProductVariant()
        {
            Id = Guid.CreateVersion7(),
            ProductId = request.ProductId,
            Name = request.Name,
            Price = request.Price,
            IsActive = true
        };
        if (request.ModifierOptionIds != null)
        {
            var modifierOptions = await _unitOfWork.GetRepository<ModifierOption>().GetListAsync(
                predicate: x => request.ModifierOptionIds.Contains(x.Id) && x.ModifierGroup.SupplierId == supplier.Id
            );
            
            if(modifierOptions.Count != request.ModifierOptionIds.Count)
            {
                throw new BadHttpRequestException("Một hoặc nhiều tùy chọn bổ sung không tồn tại hoặc bạn không có quyền sử dụng chúng");
            }

            productVariant.ProductVariantOptions = modifierOptions.Select(x => new ProductVariantOption()
            {
                Id = Guid.CreateVersion7(),
                ProductVariantId = productVariant.Id,
                ModifierOptionId = x.Id,
            }).ToList();
            
        }
        await _unitOfWork.GetRepository<ProductVariant>().InsertAsync(productVariant);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Có lỗi xảy ra khi tạo biến thể sản phẩm");
        }
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Tạo biến thể sản phẩm thành công",
            Data = productVariant.Id
        };
    }
}