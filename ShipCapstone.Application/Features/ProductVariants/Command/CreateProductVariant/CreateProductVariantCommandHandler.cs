using Mediator;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ProductVariants.Command.CreateProductVariant;

public class CreateProductVariantCommandHandler : IRequestHandler<CreateProductVariantCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;

    public CreateProductVariantCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }

    public async ValueTask<ApiResponse> Handle(CreateProductVariantCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Không tìm thấy thông tin người dùng");
        }

        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ProductId && x.SupplierId == accountId
        );

        if (product == null)
        {
            throw new BadHttpRequestException("Sản phẩm không tồn tại hoặc bạn không có quyền thêm biến thể cho sản phẩm này");
        }

        var productVariant = new ProductVariant()
        {
            Id = Guid.CreateVersion7(),
            ProductId = request.ProductId,
            Name = request.Name,
            Price = request.Price
        };

        await _unitOfWork.GetRepository<ProductVariant>().InsertAsync(productVariant);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Có lỗi xảy ra khi tạo biến thể sản phẩm");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Tạo biến thể sản phẩm thành công",
            Data = productVariant.Id
        };
    }
}