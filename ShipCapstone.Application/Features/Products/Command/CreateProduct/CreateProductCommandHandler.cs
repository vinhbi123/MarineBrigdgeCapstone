using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Products.Command.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    private readonly IUploadService _uploadService;
    
    public CreateProductCommandHandler(
        IUnitOfWork<ShipCapstoneContext> unitOfWork,
        ILogger logger,
        IClaimService claimService, IUploadService uploadService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        _uploadService = uploadService ?? throw new ArgumentNullException(nameof(uploadService));
    }
    
    public async ValueTask<ApiResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;

        var category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.CategoryId && x.SupplierId == accountId
        );
        if (category == null)
        {
            throw new NotFoundException("Danh mục không tồn tại");
        }

        var product = new Product()
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            Description = request.Description,
            CategoryId = category.Id,
            SupplierId = accountId,
        };

        if (request.IsHasVariant)
        {
            if (request.ProductVariants == null || !request.ProductVariants.Any())
            {
                throw new BadHttpRequestException("Sản phẩm có biến thể phải có ít nhất một biến thể");
            }

            product.IsHasVariant = true;
            product.ProductVariants = request.ProductVariants.Select(variant => new ProductVariant
            {
                Id = Guid.CreateVersion7(),
                Name = variant.Name,
                Price = variant.Price,
                ProductId = product.Id,
                Inventories = new List<Inventory>()
                {
                    new Inventory()
                    {
                        Id = Guid.CreateVersion7(),
                        Quantity = 0,
                        ModifierOptionId = null
                    }
                }
            }).ToList();
        }
        else
        {
            if (request.Price == null)  
            {
                throw new BadHttpRequestException("Sản phẩm không có biến thể phải có giá");
            }

            product.IsHasVariant = false;
            product.ProductVariants = new List<ProductVariant>
            {
                new ProductVariant
                {
                    Id = Guid.CreateVersion7(),
                    Name = product.Name,
                    Price = request.Price.Value,
                    ProductId = product.Id,
                    Inventories = new List<Inventory>()
                    {
                        new Inventory()
                        {
                            Id = Guid.CreateVersion7(),
                            Quantity = 0,
                            ModifierOptionId = null
                        }
                    }
                }
            };
        }
        
        var productImages = new List<ProductImage>();
        var productImageUploadTasks = request.ProductImages.Select(async productImageUpload =>
        {
            var uploadResult = await _uploadService.UploadImageAsync(productImageUpload);
            productImages.Add(new ProductImage
            {
                Id = Guid.CreateVersion7(),
                ImageUrl = uploadResult,
                //SortOrder = productImageUpload.SortOrder,
                ProductId = product.Id
            });
        });
        await Task.WhenAll(productImageUploadTasks);
        product.ProductImages = productImages;
        
        await _unitOfWork.GetRepository<Product>().InsertAsync(product);

        var isSuccess = await _unitOfWork.CommitAsync() > 0;

        if (!isSuccess)
        {
            throw new Exception("Một lỗi đã xảy ra trong quá trình tạo sản phẩm");
        }

        return new ApiResponse
        {
            Status = StatusCodes.Status201Created,
            Message = "Tạo sản phẩm thành công",
            Data = product.Id
        };
    }
}