using Mediator;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Review;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Reviews.Query.GetAllReview;

public class GetAllReviewQueryHandler : IRequestHandler<GetAllReviewQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;

    public GetAllReviewQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    public async ValueTask<ApiResponse> Handle(GetAllReviewQuery request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
            predicate: p => p.Id == request.Id) ?? throw new BadHttpRequestException("Không tìm thấy sản phẩm");
        var reviews = await _unitOfWork.GetRepository<Review>().GetPagingListAsync(
            selector: r => new GetReviewResponse()
            {
                Id = r.Id,
                AccountId = r.AccountId,
                AccountName = r.Account.FullName,
                ProductId = r.ProductId,
                ProductName = product.Name,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedDate = r.CreatedDate
            },
            predicate: r => r.ProductId == product.Id,
            page: request.Page,
            size: request.Size,
            sortBy: request.SortBy ?? nameof(Review.CreatedDate),
            isAsc: request.IsAsc);

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy danh sách đánh giá sản phẩm thành công",
            Data = reviews
        };
    }
}