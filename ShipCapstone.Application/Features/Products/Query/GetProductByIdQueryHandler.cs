using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Products;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Products.Query.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ApiResponse<GetProductResponse>>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;

    public GetProductByIdQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask<ApiResponse<GetProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.GetRepository<Product>()
            .SingleOrDefaultAsync(
                predicate: p => p.Id == request.ProductId,
                include: q => q
                    .Include(p => p.Category)
                    .Include(p => p.ProductVariants)
                    .Include(p => p.ProductImages)
            );

        if (product == null)
            throw new NotFoundException("Không tìm thấy sản phẩm");

        var response = new GetProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name,
            SupplierId = product.SupplierId,
            Price = product.ProductVariants != null && product.ProductVariants.Any() ? (decimal?)null : product.ProductVariants?.FirstOrDefault()?.Price ?? null, // best-effort
            IsHasVariant = product.ProductVariants != null && product.ProductVariants.Any(),
            Variants = product.ProductVariants?.Select(v => new ProductVariantResponse
            {
                Id = v.Id,
                Name = v.Name,
                Price = v.Price
            }).ToList(),
            Images = product.ProductImages?.Select(i => new ProductImageResponse
            {
                Id = i.Id,
                Url = i.Url
            }).ToList(),
            CreatedDate = product.CreatedDate,
            LastModifiedDate = product.LastModifiedDate
        };

        return new ApiResponse<GetProductResponse>
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy sản phẩm thành công",
            Data = response
        };
    }
}
