using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Review;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Reviews.Command.CreateReview;

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly IClaimService _claimService;

    public CreateReviewCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    public async ValueTask<ApiResponse> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: a => a.Id == accountId) ?? throw new NotFoundException("Không tìm thấy tài khoản");
        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
            predicate: p => p.Id == request.Id) ?? throw new NotFoundException("Không tìm thấy sản phẩm");
        var orderItemsExist = await _unitOfWork.GetRepository<OrderItem>().GetListAsync(
            predicate: oi => oi.ProductVariant.ProductId == product.Id 
                             && (oi.Order.Ship.AccountId == accountId || oi.Order.Boatyard.AccountId == accountId)
                             && oi.Order.Status != EOrderStatus.Pending 
                             && oi.Order.Status != EOrderStatus.Rejected);
        if (orderItemsExist == null || orderItemsExist.Count == 0)
        {
            throw new BadHttpRequestException("Bạn chưa mua sản phẩm này nên không thể đánh giá");
        }

        var review = new Review()
        {
            Id = Guid.CreateVersion7(),
            AccountId = accountId,
            ProductId = product.Id,
            Rating = request.Rating,
            Comment = request.Comment
        };
        await _unitOfWork.GetRepository<Review>().InsertAsync(review);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Có lỗi xảy ra trong quá trình đánh giá sản phẩm");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Đánh giá sản phẩm thành công",
            Data = new CreateReviewResponse()
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                ProductId = review.ProductId,
                AccountId = review.AccountId
            }
        };
    }
}