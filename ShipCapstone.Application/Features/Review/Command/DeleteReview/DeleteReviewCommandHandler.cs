using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Features.Review.Command.DeleteReview;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;
// alias để tránh trùng tên namespace/class nếu cần
using ReviewEntity = ShipCapstone.Domain.Entities.Review;

namespace ShipCapstone.Application.Features.Reviews.Command.DeleteReview;

public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;

    public DeleteReviewCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }

    public async ValueTask<ApiResponse> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _claimService.GetCurrentUserId;
        if (currentUserId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }

        var review = await _unitOfWork.GetRepository<ReviewEntity>()
            .SingleOrDefaultAsync(predicate: r => r.Id == request.ReviewId);

        if (review == null)
            throw new NotFoundException("Không tìm thấy đánh giá");

        var isOwner = review.AccountId == currentUserId;
        var isAdmin = false;

        try
        {
            var roleString = _claimService.GetRole;
            if (!string.IsNullOrWhiteSpace(roleString) && Enum.TryParse<ERole>(roleString, true, out var parsedRole))
            {
                isAdmin = parsedRole == ERole.Admin;
            }
        }
        catch
        {
            isAdmin = false;
        }

        if (!isOwner && !isAdmin)
        {
            throw new BadHttpRequestException("Bạn không có quyền xóa đánh giá này");
        }

        _unitOfWork.GetRepository<ReviewEntity>().DeleteAsync(review);

        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Xóa đánh giá thất bại");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Xóa đánh giá thành công",
            Data = request.ReviewId
        };
    }
}
