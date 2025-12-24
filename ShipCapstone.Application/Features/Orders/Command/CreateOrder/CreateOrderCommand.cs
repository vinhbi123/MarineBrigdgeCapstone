    using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Orders.Command.CreateOrder
{
    public class CreateOrderCommand : IRequest<ApiResponse>
    {
        public Guid ShipId { get; set; }
        public List<CreateOrderItemRequest> OrderItems { get; set; }
    }
    public class CreateOrderItemRequest
    {
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public string? ProductOptionName { get; set; }
    }
}
