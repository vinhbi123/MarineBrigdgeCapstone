using Mediator;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Orders.Command.UpdateOrder;

public class UpdateOrderCommand : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
    public EOrderStatus? Status { get; set; }
    public List<UpdateOrderItemRequest>? OrderItems { get; set; }
}

public class UpdateOrderRequest
{
    public EOrderStatus? Status { get; set; }
    public List<UpdateOrderItemRequest>? OrderItems { get; set; }
}

public class UpdateOrderItemRequest
{
    public Guid? Id { get; set; }
    public int? Quantity { get; set; }
}