    using Mediator;
    using Microsoft.EntityFrameworkCore;
    using ShipCapstone.Application.Common.Exceptions;
    using ShipCapstone.Application.Services.Interfaces;
    using ShipCapstone.Domain.Entities;
    using ShipCapstone.Domain.Enums;
    using ShipCapstone.Domain.Models.Common;
    using ShipCapstone.Domain.Models.Orders;
    using ShipCapstone.Infrastructure.Persistence;
    using ShipCapstone.Infrastructure.Repositories.Interface;

    namespace ShipCapstone.Application.Features.Orders.Command.CreateOrder
    {
        public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResponse>
        {
            private readonly IClaimService _claimService;
            private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
            public CreateOrderCommandHandler(IClaimService claimService, IUnitOfWork<ShipCapstoneContext> unitOfWork)
            {
                _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
                _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            }
            public async ValueTask<ApiResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                var accountId = _claimService.GetCurrentUserId;
                var role = Enum.Parse<ERole>(_claimService.GetRole);
                if (accountId == Guid.Empty)
                {
                    throw new BadHttpRequestException("Không tìm thấy tài khoản.");
                }
                var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                    predicate: a => a.Id == accountId,
                    include: a => a.Include(a => a.Ships)
                        .Include(a => a.Boatyard)) ?? throw new NotFoundException("Không tìm thấy tài khoản.");
                var order = new Order();
                decimal totalAmount = 0;
                var orderItems = new List<OrderItem>();
                if (role == ERole.User)
                {
                    var ship = await _unitOfWork.GetRepository<Ship>().SingleOrDefaultAsync(
                        predicate: s => s.Id == request.ShipId) ?? throw new NotFoundException("Không tìm thấy tàu");
                    if (!account.Ships.Any(s => s.AccountId == accountId))
                    {
                        throw new BadHttpRequestException("Tàu này không thuộc về bạn");
                    }

                    order = new Order
                    {
                        Id = Guid.CreateVersion7(),
                        ShipId = ship.Id,
                        TotalAmount = totalAmount,
                        Status = EOrderStatus.Pending
                    };
                }
                else
                {
                    order = new Order
                    {
                        Id = Guid.CreateVersion7(),
                        BoatyardId = account.Boatyard.Id,
                        TotalAmount = totalAmount,
                        Status = EOrderStatus.Pending
                    };
                }
                
                await _unitOfWork.GetRepository<Order>().InsertAsync(order);
                foreach(var item in request.OrderItems)
                {
                    var productVariant = await _unitOfWork.GetRepository<ProductVariant>().SingleOrDefaultAsync(
                        predicate: pv => pv.Id == item.ProductVariantId) ?? throw new NotFoundException("Không tìm thấy sản phẩm");
                    var orderItem = new OrderItem()
                    {
                        Id = Guid.CreateVersion7(),
                        Price = productVariant.Price,
                        ProductOptionName = item.ProductOptionName,
                        Quantity = item.Quantity,
                        ProductVariantId = productVariant.Id,
                        OrderId = order.Id
                    };
                totalAmount += productVariant.Price * item.Quantity;
                orderItems.Add(orderItem);
                };
                await _unitOfWork.GetRepository<OrderItem>().InsertRangeAsync(orderItems);
                order.TotalAmount = totalAmount;
                var isSuccess = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccess)
                {
                    throw new Exception("Có lỗi khi tạo đơn hàng");
                }
                return new ApiResponse
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Tạo đơn hàng thành công",
                    Data = new CreateOrderResponse
                    {
                        Id = order.Id,
                        ShipId = order.ShipId,
                        BoatyardId = order.BoatyardId,
                        TotalAmount = order.TotalAmount,
                        Status = order.Status
                    }
                };
            }

        }
    }
