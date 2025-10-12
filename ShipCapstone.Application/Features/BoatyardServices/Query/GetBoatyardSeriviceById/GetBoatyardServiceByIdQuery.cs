using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.BoatyardServices.Query.GetBoatyardSeriviceById;

public class GetBoatyardServiceByIdQuery : IRequest<ApiResponse>
{
    public Guid BoatyardServiceId { get; set; }
}