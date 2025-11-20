using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ShipCapstone.Application.Features.Products.Command.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;

    public DeleteProductCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }

    public async ValueTask<ApiResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _claimService.GetCurrentUserId;
        if (currentUserId == Guid.Empty)
            throw new BadHttpRequestException("Tài khoản không hợp lệ");

        // load product with related variants/images to remove them explicitly if needed
        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
            predicate: p => p.Id == request.ProductId,
            include: q => q.Include(p => p.ProductVariants).Include(p => p.ProductImages)
        );

        if (product == null)
            throw new NotFoundException("Không tìm thấy sản phẩm");

        // check permission: supplier owner or admin
        var isOwner = product.SupplierId == currentUserId;
        var isAdmin = false;
        try
        {
            var roleString = _claimService.GetRole;
            if (!string.IsNullOrWhiteSpace(roleString) && Enum.TryParse<ERole>(roleString, true, out var parsed))
                isAdmin = parsed == ERole.Admin;
        }
        catch
        {
            isAdmin = false;
        }

        if (!isOwner && !isAdmin)
            throw new BadHttpRequestException("Bạn không có quyền xóa sản phẩm này");

        // delete variants
        var variantRepo = _unitOfWork.GetRepository<ProductVariant>();
        if (product.ProductVariants != null && product.ProductVariants.Any())
        {
            foreach (var v in product.ProductVariants.ToList())
            {
                // await if DeleteAsync returns Task
                await variantRepo.DeleteAsync(v);
            }
        }

        // delete images
        var imageRepo = _unitOfWork.GetRepository<ProductImage>();
        if (product.ProductImages != null && product.ProductImages.Any())
        {
            foreach (var img in product.ProductImages.ToList())
            {
                await imageRepo.DeleteAsync(img);
            }
        }

        // delete product
        await _unitOfWork.GetRepository<Product>().DeleteAsync(product);

        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
            throw new Exception("Xóa sản phẩm thất bại");

        return new ApiResponse
        {
            Status = StatusCodes.Status200OK,
            Message = "Xóa sản phẩm thành công",
            Data = request.ProductId
        };
    }
}
