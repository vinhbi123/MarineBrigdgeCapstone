using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Orders.Command.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResponse>
{
    private readonly IClaimService _claimService;
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;

    public UpdateOrderCommandHandler(IClaimService claimService, IUnitOfWork<ShipCapstoneContext> unitOfWork)
    {
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    public async ValueTask<ApiResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        var role = Enum.Parse<ERole>(_claimService.GetRole);
        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: a => a.Id == accountId,
            include: a => a.Include(a => a.Boatyard)) ?? throw new NotFoundException("Không tìm thấy tài khoản");

        var order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
            predicate: o => o.Id == request.Id,
            include: o => o.Include(o => o.Ship)) ?? throw new NotFoundException("Không tìm thấy đơn hàng");
        decimal totalAmount = 0;
        if (role != ERole.Supplier && request.Status != null)
        {
            throw new BadHttpRequestException("Bạn không có quyền chỉnh sửa trạng thái đơn hàng");
        }

        if (role == ERole.User)
        {
            if (order.Ship.AccountId != accountId)
            {
                throw new BadHttpRequestException("Người dùng không sở hữu đơn hàng này");
            }
            if (order.Status != EOrderStatus.Pending)
            {
                throw new BadHttpRequestException("Bạn chỉ có thể cập nhật đơn hàng khi chưa thanh toán");
            }
        }

        else if (role == ERole.Boatyard)
        {
            if (order.BoatyardId != account.Boatyard?.Id)
            {
                throw new BadHttpRequestException("Đơn hàng này không thuộc boathouse của bạn");
            }

            if (order.Status != EOrderStatus.Pending)
            {
                throw new BadHttpRequestException("Bạn chỉ có thể cập nhật đơn hàng khi chưa thanh toán");
            }
        }

        if (request.OrderItems != null)
        {
            foreach (var itemRequest in request.OrderItems)
            {
                var item = await _unitOfWork.GetRepository<OrderItem>().SingleOrDefaultAsync(
                    predicate: i => i.Id == itemRequest.Id && i.OrderId == order.Id) ?? throw new BadHttpRequestException("Không tìm thấy sản phẩm trong đơn hàng");
                item.Quantity = itemRequest.Quantity ?? item.Quantity;
                totalAmount += item.Price * item.Quantity;
                _unitOfWork.GetRepository<OrderItem>().UpdateAsync(item);
            }
        }

        order.Status = request.Status ?? order.Status;
        order.TotalAmount = totalAmount == 0 ? order.TotalAmount : totalAmount;
        _unitOfWork.GetRepository<Order>().UpdateAsync(order);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Có lỗi xảy ra trong quá trình cập nhật đơn hàng");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Cập nhật đơn hàng thành công",
            Data = order.Id
        };
    }
}