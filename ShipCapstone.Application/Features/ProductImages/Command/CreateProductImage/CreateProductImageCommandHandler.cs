using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ProductImages.Command.CreateProductImage;

public class CreateProductImageCommandHandler : IRequestHandler<CreateProductImageCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    private readonly IUploadService _uploadService;
    
    public CreateProductImageCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger, IClaimService claimService,
        IUploadService uploadService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        _uploadService = uploadService ?? throw new ArgumentNullException(nameof(uploadService));
    }
    
    public async ValueTask<ApiResponse> Handle(CreateProductImageCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Không tìm thấy thông tin người dùng");
        }

        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ProductId 
                            && x.Category.Supplier.AccountId == accountId
        );
        
        if (product == null)
        {
            throw new BadHttpRequestException("Sản phẩm không tồn tại hoặc bạn không có quyền thêm hình ảnh cho sản phẩm này");
        }
        
        var imageUrl = await _uploadService.UploadImageAsync(request.Image);

        var productImage = new ProductImage()
        {
            Id = Guid.CreateVersion7(),
            ProductId = request.ProductId,
            ImageUrl = imageUrl,
            SortOrder = request.SortOrder
        };
        
        await _unitOfWork.GetRepository<ProductImage>().InsertAsync(productImage);
        
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Tạo hình ảnh sản phẩm thất bại");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Tạo hình ảnh sản phẩm thành công",
            Data = product.Id
        };
    }
}