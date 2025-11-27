using Mediator;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Products;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Products.Query.GetProductsBySupplierId;

public class GetProductsBySupplierIdQueryHandler : IRequestHandler<GetProductsBySupplierIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;

    public GetProductsBySupplierIdQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork,
        ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask<ApiResponse> Handle(GetProductsBySupplierIdQuery request, CancellationToken cancellationToken)
    {
        var products = await _unitOfWork.GetRepository<Product>().GetPagingListAsync(
            selector: x => new GetProductsBySupplierIdResponse()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                SupplierId = x.SupplierId,
                SupplierName = x.Category.Supplier.Name,
                IsHasVariant = x.IsHasVariant,
                ImageUrl = x.ProductImages.OrderBy(pi => pi.SortOrder ?? int.MaxValue)
                    .Select(x => x.ImageUrl).FirstOrDefault(),
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate
            },
            predicate: x => x.SupplierId == request.SupplierId
                            && (string.IsNullOrEmpty(request.Name) || x.Name.Contains(request.Name)),
            page: request.Page,
            size: request.Size,
            sortBy: request.SortBy ?? nameof(Product.CreatedDate),
            isAsc: request.IsAsc
        );

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy danh sách sản phẩm theo nhà cung cấp thành công",
            Data = products
        };
    }
}