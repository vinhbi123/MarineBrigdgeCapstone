using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Products;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Products.Query.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    
    public GetProductByIdQueryHandler(
        IUnitOfWork<ShipCapstoneContext> unitOfWork, 
        ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async ValueTask<ApiResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ProductId,
            include: x => x.Include(x => x.ProductImages)
                .Include(x => x.ProductVariants)
                .Include(x => x.Category)
                .ThenInclude(x => x.Supplier)
        );

        var response = new GetProductByIdResponse()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            CategoryId = product.CategoryId,
            CategoryName = product.Category.Name,
            SupplierId = product.SupplierId,
            SupplierName = product.Category.Supplier.Name,
            IsHasVariant = product.IsHasVariant,
            ProductImages = product.ProductImages.Select(pi => new ProductImageResponse()
            {
                Id = pi.Id,
                ImageUrl = pi.ImageUrl,
                SortOrder = pi.SortOrder
            }).ToList(),
            ProductVariants = product.ProductVariants.Select(pv => new ProductVariantResponseForGetProductById()
            {
                Id = pv.Id,
                Name = pv.Name,
                Price = pv.Price
            }).ToList()
        };
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy thông tin sản phẩm thành công",
            Data = response
        };

    }
}