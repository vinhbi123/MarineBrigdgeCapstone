using Mediator;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Products;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Products.Query.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public GetProductsQueryHandler(
        IUnitOfWork<ShipCapstoneContext> unitOfWork, 
        ILogger logger,
        IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    
    public async ValueTask<ApiResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        Enum.TryParse<ERole>(_claimService.GetRole, out var parsedRole);

        if (parsedRole == ERole.Supplier)
        {
            request.SupplierId = null;
        }
        
        var products = await _unitOfWork.GetRepository<Product>().GetPagingListAsync(
            selector: x => new GetProductsResponse()
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
                IsActive = x.IsActive,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate
            },
            predicate: x => (string.IsNullOrEmpty(request.Name) || x.Name.Contains(request.Name)) 
                            && (request.SupplierId == null || x.SupplierId == request.SupplierId)
                            && (parsedRole != ERole.Supplier || x.Category.Supplier.AccountId == _claimService.GetCurrentUserId)
                            && (parsedRole != ERole.User || parsedRole != ERole.Boatyard || parsedRole != ERole.Captain || x.IsActive),
            page: request.Page,
            size: request.Size,
            sortBy: request.SortBy ?? nameof(Product.CreatedDate),
            isAsc: request.IsAsc
        );
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy danh sách sản phẩm thành công",
            Data = products
        };
    }
}