using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Orders;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;
using System.Linq;

namespace ShipCapstone.Application.Features.Orders.Query
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, ApiResponse>
    {
        private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;

        public GetAllOrdersQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async ValueTask<ApiResponse> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.GetRepository<Order>().GetPagingListAsync(
                selector: o => new GetAllOrderResponse
                {
                    Id = o.Id,
                    ShipId = o.ShipId,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status
                },
                predicate: o =>
                    (!request.ShipId.HasValue || o.ShipId == request.ShipId) &&
                    (string.IsNullOrEmpty(request.Status) || o.Status.ToString() == request.Status) &&
                    (string.IsNullOrEmpty(request.Search) || o.Id.ToString().Contains(request.Search)),
                page: request.Page,
                size: request.PageSize,
                sortBy: request.SortBy ?? nameof(Order.CreatedDate),
                isAsc: request.IsAsc
            ) ?? throw new NotFoundException("Không tìm thấy đơn hàng.");

            return new ApiResponse
            {
                Status = StatusCodes.Status200OK,
                Message = "Lấy danh sách đơn hàng thành công",
                Data = orders
            };
        }
    }
}