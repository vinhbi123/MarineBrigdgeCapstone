using Mediator;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Products.Command.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;

    public UpdateProductCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger,
        IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }


    public async ValueTask<ApiResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Người dùng không hợp lệ");
        }

        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ProductId && x.SupplierId == accountId
        );

        if (product == null)
        {
            throw new BadHttpRequestException("Sản phẩm không tồn tại hoặc bạn không có quyền cập nhật sản phẩm này");
        }

        product.Name = request.Name ?? product.Name;
        product.Description = request.Description ?? product.Description;

        if (request.CategoryId != null && request.CategoryId != product.CategoryId)
        {
            var category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id == request.CategoryId && x.SupplierId == accountId
            );
            if (category == null)
            {
                throw new BadHttpRequestException("Danh mục không tồn tại hoặc bạn không có quyền sử dụng danh mục này");
            }
            product.CategoryId = category.Id;
        }
        _unitOfWork.GetRepository<Product>().UpdateAsync(product);

        var isSuccess = await _unitOfWork.CommitAsync() > 0;

        if (!isSuccess)
        {
            throw new Exception("Cập nhật sản phẩm thất bại");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Cập nhật sản phẩm thành công",
            Data = product.Id
        };
    }
}