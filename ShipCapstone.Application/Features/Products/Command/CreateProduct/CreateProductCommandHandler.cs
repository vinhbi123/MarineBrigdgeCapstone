using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Products.Command.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;

    public CreateProductCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask<ApiResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // validate category exists
        var category = await _unitOfWork.GetRepository<Category>()
            .SingleOrDefaultAsync(predicate: c => c.Id == request.CategoryId);
        if (category == null)
            throw new NotFoundException("Không tìm thấy category");

        // optional: validate supplier exists
        var supplier = await _unitOfWork.GetRepository<Supplier>()
            .SingleOrDefaultAsync(predicate: s => s.Id == request.SupplierId);
        if (supplier == null)
            throw new NotFoundException("Không tìm thấy supplier");

        var product = new Product
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            Description = request.Description,
            CategoryId = request.CategoryId,
            SupplierId = request.SupplierId,
            // keep audit fields handled by EF / base class
        };

        // insert product first to get its Id
        await _unitOfWork.GetRepository<Product>().InsertAsync(product);

        // handle variants
        if (request.IsHasVariant && request.Variants != null && request.Variants.Count > 0)
        {
            var variantRepo = _unitOfWork.GetRepository<ProductVariant>();
            foreach (var v in request.Variants)
            {
                var variant = new ProductVariant
                {
                    Id = Guid.CreateVersion7(),
                    ProductId = product.Id,
                    Name = v.Name,
                    Price = v.Price
                };
                await variantRepo.InsertAsync(variant);
            }
        }
        else
        {
            // no variants: if Price provided, create a default variant to store price
            if (request.Price.HasValue)
            {
                var variantRepo = _unitOfWork.GetRepository<ProductVariant>();
                var defaultVariant = new ProductVariant
                {
                    Id = Guid.CreateVersion7(),
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = request.Price.Value
                };
                await variantRepo.InsertAsync(defaultVariant);
            }
        }

        // handle images
        if (request.Images != null && request.Images.Count > 0)
        {
            var imageRepo = _unitOfWork.GetRepository<ProductImage>();
            foreach (var img in request.Images)
            {
                var pi = new ProductImage
                {
                    Id = Guid.CreateVersion7(),
                    ProductId = product.Id,
                    Url = img.Url
                };
                await imageRepo.InsertAsync(pi);
            }
        }

        var saved = await _unitOfWork.CommitAsync() > 0;
        if (!saved)
            throw new Exception("Tạo sản phẩm thất bại");

        return new ApiResponse
        {
            Status = StatusCodes.Status201Created,
            Message = "Tạo sản phẩm thành công",
            Data = product.Id
        };
    }
}
