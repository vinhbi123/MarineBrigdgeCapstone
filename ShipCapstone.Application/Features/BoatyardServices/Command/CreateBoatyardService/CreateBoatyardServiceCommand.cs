using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.BoatyardServices.Command.CreateBoatyardService;

public class CreateBoatyardServiceCommand : IRequest<ApiResponse>
{
    public string TypeService { get; set; }
    public decimal Price { get; set; }
}