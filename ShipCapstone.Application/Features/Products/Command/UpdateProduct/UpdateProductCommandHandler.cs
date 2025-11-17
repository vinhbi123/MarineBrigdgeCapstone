using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Products.Command.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;

    public UpdateProductCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask<ApiResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var productRepo = _unitOfWork.GetRepository<Product>();

        var product = await productRepo.SingleOrDefaultAsync(
            predicate: p => p.Id == request.ProductId,
            include: q => q.Include(p => p.ProductVariants).Include(p => p.ProductImages)
        );

        if (product == null)
            throw new NotFoundException("Không tìm thấy sản phẩm");

        // validate category
        var category = await _unitOfWork.GetRepository<Category>()
            .SingleOrDefaultAsync(predicate: c => c.Id == request.CategoryId);
        if (category == null)
            throw new NotFoundException("Không tìm thấy category");

        product.Name = request.Name;
        product.Description = request.Description;
        product.CategoryId = request.CategoryId;
        product.SupplierId = category.SupplierId; 
        product.LastModifiedDate = DateTime.UtcNow;

        var variantRepo = _unitOfWork.GetRepository<ProductVariant>();
        var existingVariants = product.ProductVariants?.ToList() ?? new List<ProductVariant>();

        if (request.IsHasVariant)
        {
            var incomingWithId = request.Variants?.Where(v => v.Id.HasValue).ToDictionary(v => v.Id!.Value) ?? new Dictionary<Guid, UpdateProductVariantRequest>();
            var incomingWithoutId = request.Variants?.Where(v => !v.Id.HasValue).ToList() ?? new List<UpdateProductVariantRequest>();

            foreach (var exist in existingVariants)
            {
                if (incomingWithId.TryGetValue(exist.Id, out var incoming))
                {
                    exist.Name = incoming.Name;
                    exist.Price = incoming.Price;
                    variantRepo.Update(exist);
                }
            }

            // Create new variants
            foreach (var inc in incomingWithoutId)
            {
                var newVar = new ProductVariant
                {
                    Id = Guid.CreateVersion7(),
                    ProductId = product.Id,
                    Name = inc.Name,
                    Price = inc.Price
                };
                await variantRepo.InsertAsync(newVar);
            }

            var incomingIds = request.Variants?.Where(v => v.Id.HasValue).Select(v => v.Id!.Value).ToHashSet() ?? new HashSet<Guid>();
            var toDelete = existingVariants.Where(ev => !incomingIds.Contains(ev.Id)).ToList();
            foreach (var del in toDelete)
            {
                await _unitOfWork.GetRepository<ProductVariant>().DeleteAsync(del);
            }
        }
        else
        {
            // client says no variants -> delete all existing, optionally create a default one from Price
            foreach (var ev in existingVariants)
            {
                await _unitOfWork.GetRepository<ProductVariant>().DeleteAsync(ev);
            }

            if (request.Price.HasValue)
            {
                var defaultVar = new ProductVariant
                {
                    Id = Guid.CreateVersion7(),
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = request.Price.Value
                };
                await variantRepo.InsertAsync(defaultVar);
            }
        }

        
        var imageRepo = _unitOfWork.GetRepository<ProductImage>();
        var existingImages = product.ProductImages?.ToList() ?? new List<ProductImage>();

        var incomingImgWithId = request.Images?.Where(i => i.Id.HasValue).ToDictionary(i => i.Id!.Value) ?? new Dictionary<Guid, UpdateProductImageRequest>();
        var incomingImgWithoutId = request.Images?.Where(i => !i.Id.HasValue).ToList() ?? new List<UpdateProductImageRequest>();

        foreach (var exist in existingImages)
        {
            if (incomingImgWithId.TryGetValue(exist.Id, out var incoming))
            {
                exist.Url = incoming.Url;
                imageRepo.Update(exist);
            }
        }

        foreach (var inc in incomingImgWithoutId)
        {
            var newImg = new ProductImage
            {
                Id = Guid.CreateVersion7(),
                ProductId = product.Id,
                Url = inc.Url
            };
            await imageRepo.InsertAsync(newImg);
        }

        var incomingImgIds = request.Images?.Where(i => i.Id.HasValue).Select(i => i.Id!.Value).ToHashSet() ?? new HashSet<Guid>();
        var imgsToDelete = existingImages.Where(ei => !incomingImgIds.Contains(ei.Id)).ToList();
        foreach (var del in imgsToDelete)
        {
            await _unitOfWork.GetRepository<ProductImage>().DeleteAsync(del);
        }

        productRepo.Update(product);

        var saved = await _unitOfWork.CommitAsync() > 0;
        if (!saved)
            throw new Exception("Cập nhật sản phẩm thất bại");

        return new ApiResponse
        {
            Status = StatusCodes.Status200OK,
            Message = "Cập nhật sản phẩm thành công",
            Data = product.Id
        };
    }
}
