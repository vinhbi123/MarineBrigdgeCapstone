using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Orders;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Orders.Query
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetAllOrderQuery, ApiResponse>
    {
        private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;

        public GetOrderByIdQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async ValueTask<ApiResponse> Handle(GetAllOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.GetRepository<Order>()
                .SingleOrDefaultAsync<Order>(
                    selector: o => o,
                    predicate: o => o.Id == request.Id,
                    orderBy: null,
                    include: null
                );

            if (order == null)
                throw new NotFoundException("Không tìm thấy đơn hàng.");

            var data = new GetOrderResponse
            {
                Id = order.Id,
                ShipId = order.ShipId,
                BoatyardId = order.BoatyardId,
                TotalAmount = order.TotalAmount,
                Status = order.Status
            };

            return new ApiResponse
            {
                Status = StatusCodes.Status200OK,
                Message = "Lấy chi tiết đơn hàng thành công",
                Data = data
            };
        }
    }
}