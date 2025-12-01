using System.Text.RegularExpressions;
using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;
using ShipCapstone.Infrastructure.Utils;

namespace ShipCapstone.Application.Features.Revenues.Command.HandleTransactionRevenue;

public class HandleTransactionRevenueCommandHandler : IRequestHandler<HandleTransactionRevenueCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;

    public HandleTransactionRevenueCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async ValueTask<ApiResponse> Handle(HandleTransactionRevenueCommand request, CancellationToken cancellationToken)
    {
        var match = Regex.Match(request.Content, @"(TXN\d{12,14})");
        if (!match.Success)
            throw new NotFoundException("Không tìm thấy mã TXN");
        var referenceCode = "SEVQR Chuyen tien " + match.Groups[1].Value;
        var transaction = await _unitOfWork.GetRepository<Transaction>().SingleOrDefaultAsync(
            predicate: t => t.TransactionCode.Replace("-", "").Equals(referenceCode)) ?? throw new NotFoundException("Không tìm thấy giao dịch");

        if (transaction.Status == ETransactionStatus.Approved)
        {
            return new ApiResponse()
            {
                Status = StatusCodes.Status200OK,
                Message = "Giao dịch đã xác nhận trước đó",
                Data = true
            };
        }

        transaction.Status = ETransactionStatus.Approved;
        transaction.LastModifiedDate = TimeUtil.GetCurrentSEATime();

        _unitOfWork.GetRepository<Transaction>().UpdateAsync(transaction);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Có lỗi xảy ra trong quá trình xác nhận thanh toán");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Xác nhận thanh toán",
            Data = true
        };
    }
}