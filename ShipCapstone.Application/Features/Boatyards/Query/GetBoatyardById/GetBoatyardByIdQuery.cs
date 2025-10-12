using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Boatyards.Query.GetBoatyardById;

public class GetBoatyardByIdQuery : IRequest<ApiResponse>
{
    public Guid BoatyardId { get; set; }
}