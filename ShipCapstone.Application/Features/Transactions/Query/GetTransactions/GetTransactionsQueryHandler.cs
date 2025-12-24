using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Transactions;
using ShipCapstone.Infrastructure.Paginate;
using ShipCapstone.Infrastructure.Paginate.Interface;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Transactions.Query.GetTransactions;

public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly IClaimService _claimService;

    public GetTransactionsQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    public async ValueTask<ApiResponse> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: a => a.Id.Equals(accountId)) ?? throw new NotFoundException("Không tìm thấy tài khoản");

        IPaginate<GetTransactionsResponse> transactions = new Paginate<GetTransactionsResponse>();
        if (account.Role.Equals(ERole.User))
        {
            transactions = await _unitOfWork.GetRepository<Transaction>().GetPagingListAsync(
                selector: t => new GetTransactionsResponse()
                {
                    Id = t.Id,
                    TransactionReference = t.TransactionCode,
                    Amount = t.Amount,
                    Status = t.Status,
                    Type = t.Type,
                    CreatedDate = t.CreatedDate,
                    LastModifiedDate = t.LastModifiedDate
                },
                predicate: t => t.Order.Ship.AccountId == accountId || t.Booking.Ship.AccountId == accountId
                    && t.Status != ETransactionStatus.Pending && t.Status != ETransactionStatus.Rejected,
                page: request.Page,
                size: request.Size,
                sortBy: request.SortBy ?? nameof(Transaction.CreatedDate),
                isAsc: request.IsAsc);
        }
        else if (account.Role.Equals(ERole.Boatyard))
        {
            var boatyard = await _unitOfWork.GetRepository<Boatyard>().SingleOrDefaultAsync(
                predicate: g => g.AccountId.Equals(accountId)) ?? throw new NotFoundException("Không tìm thấy thông tin xưởng sửa chữa");
            transactions = await _unitOfWork.GetRepository<Transaction>().GetPagingListAsync(
                selector: t => new GetTransactionsResponse()
                {
                    Id = t.Id,
                    TransactionReference = t.TransactionCode,
                    Amount = t.Amount,
                    Status = t.Status,
                    Type = t.Type,
                    CreatedDate = t.CreatedDate,
                    LastModifiedDate = t.LastModifiedDate
                },
                predicate: t => (t.Booking.BookingServices.Any(bs => bs.BoatyardService.BoatyardId == boatyard.Id) || t.Order.BoatyardId == boatyard.Id)
                                && (t.Status.Equals(ETransactionStatus.Approved) || t.Type.Equals(EPaymentType.Revenue)),
                page: request.Page,
                size: request.Size);
        }
        else if (account.Role.Equals(ERole.Admin))
        {
            transactions = await _unitOfWork.GetRepository<Transaction>().GetPagingListAsync(
                selector: t => new GetTransactionsResponse()
                {
                    Id = t.Id,
                    TransactionReference = t.TransactionCode,
                    Amount = t.Amount,
                    Status = t.Status,
                    Type = t.Type,
                    CreatedDate = t.CreatedDate,
                    LastModifiedDate = t.LastModifiedDate
                },
                predicate: t => !t.Status.Equals(ETransactionStatus.Pending) && !t.Status.Equals(ETransactionStatus.Rejected),
                page: request.Page,
                size: request.Size);
        }
        else
        {
            var supplier = await _unitOfWork.GetRepository<Supplier>().SingleOrDefaultAsync(
                predicate: g => g.AccountId.Equals(accountId)) ?? throw new NotFoundException("Không tìm thấy thông tin đại lí");
            transactions = await _unitOfWork.GetRepository<Transaction>().GetPagingListAsync(
                selector: t => new GetTransactionsResponse()
                {
                    Id = t.Id,
                    TransactionReference = t.TransactionCode,
                    Amount = t.Amount,
                    Status = t.Status,
                    Type = t.Type,
                    CreatedDate = t.CreatedDate,
                    LastModifiedDate = t.LastModifiedDate
                },
                predicate: t => t.Order.OrderItems.Any(oi => oi.ProductVariant.Product.SupplierId == supplier.Id) && (t.Status.Equals(ETransactionStatus.Approved) || t.Type.Equals(EPaymentType.Revenue)),
                page: request.Page,
                size: request.Size);
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy transaction thành công",
            Data = transactions
        };
    }
}