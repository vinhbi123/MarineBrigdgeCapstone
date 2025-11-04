using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Reviews;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;
using ShipCapstone.Infrastructure.Paginate.Interface;
using System.Linq.Expressions;

namespace ShipCapstone.Application.Features.Reviews.Query.GetReviewsByProduct;

public class GetReviewsByProductQueryHandler : IRequestHandler<GetReviewsByProductQuery, ApiResponse<IPaginate<GetReviewResponse>>>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;

    public GetReviewsByProductQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async ValueTask<ApiResponse<IPaginate<GetReviewResponse>>> Handle(GetReviewsByProductQuery request, CancellationToken cancellationToken)
    {
        // check product tồn tại
        var product = await _unitOfWork.GetRepository<Product>()
            .SingleOrDefaultAsync(predicate: p => p.Id == request.ProductId);
        if (product == null)
            throw new NotFoundException("Không tìm thấy sản phẩm");

        Expression<Func<Review, GetReviewResponse>> selector = r => new GetReviewResponse
        {
            Id = r.Id,
            Rating = r.Rating,
            Comment = r.Comment,
            AccountId = r.AccountId,
            ProductId = r.ProductId,
            AccountFullName = r.Account != null ? r.Account.FullName : null,
            AccountAvatarUrl = r.Account != null ? r.Account.AvatarUrl : null,
            CreatedDate = r.CreatedDate
        };

        Expression<Func<Review, bool>> predicate = r => r.ProductId == request.ProductId;
        Func<IQueryable<Review>, IOrderedQueryable<Review>> orderBy = q => q.OrderByDescending(r => r.CreatedDate);

        var repo = _unitOfWork.GetRepository<Review>();
        var paged = await repo.GetListAsync(
            selector: selector,
            predicate: predicate,
            orderBy: orderBy,
            include: q => q.Include(r => r.Account),
            pageIndex: request.Page,
            pageSize: request.Size,
            cancellationToken: cancellationToken
        );

        return new ApiResponse<IPaginate<GetReviewResponse>>
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy danh sách đánh giá thành công",
            Data = paged
        };
    }
}
