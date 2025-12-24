using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Products;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Products.Query.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    public GetProductByIdQueryHandler(
        IUnitOfWork<ShipCapstoneContext> unitOfWork, 
        ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        Enum.TryParse<ERole>(_claimService.GetRole, out var parsedRole);

        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ProductId &&
                       (parsedRole != ERole.Supplier || x.Category.Supplier.AccountId == _claimService.GetCurrentUserId) &&
                       (parsedRole != ERole.User || parsedRole != ERole.Boatyard || parsedRole != ERole.Captain || x.IsActive),
            include: x => x.Include(x => x.ProductImages)
                .Include(x => x.ProductVariants)
                .ThenInclude(pv => pv.ProductVariantOptions)
                .ThenInclude(pvo => pvo.ModifierOption)
                .ThenInclude(mo => mo.ModifierGroup)
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
            IsActive = product.IsActive,
            ProductImages = product.ProductImages.Select(pi => new ProductImageResponse()
            {
                Id = pi.Id,
                ImageUrl = pi.ImageUrl,
                SortOrder = pi.SortOrder
            }).ToList(),
            // ProductVariants = product.ProductVariants.Select(pv => new ProductVariantResponseForGetProductById()
            // {
            //     Id = pv.Id,
            //     Name = pv.Name,
            //     Price = pv.Price
            // }).ToList()
            ProductVariants = product.ProductVariants
                .Where(pv => parsedRole != ERole.User && parsedRole != ERole.Boatyard && parsedRole != ERole.Captain || pv.IsActive)
                .Select(pv =>
            {
                var modifierGroups = pv.ProductVariantOptions?.Select(pvo => pvo.ModifierOption)
                    .GroupBy(mo => mo.ModifierGroup.Id)
                    .Select(g => {
                        var mg = g.First().ModifierGroup;
                        return new ModifierGroupResponseForGetProductById()
                        {
                            Id = mg.Id,
                            Name = mg.Name,
                            ModifierOptions = g.Select(mo => new ModifierOptionResponseForGetProductById()
                            {
                                Id = mo.Id,
                                Name = mo.Name
                            }).ToList()
                        };
                    }).ToList();
                return new ProductVariantResponseForGetProductById()
                {
                    Id = pv.Id,
                    Name = pv.Name,
                    Price = pv.Price,
                    IsActive = pv.IsActive,
                    ModifierGroups = (modifierGroups != null && modifierGroups.Any()) ? modifierGroups : null
                };
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