using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Ports.Query.GetPortById;

public class GetPortByIdQuery : IRequest<ApiResponse>
{
    public Guid PortId { get; set; }
}