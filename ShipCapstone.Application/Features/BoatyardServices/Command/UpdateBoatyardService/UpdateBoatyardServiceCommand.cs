using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.BoatyardServices.Command.UpdateBoatyardService;

public class UpdateBoatyardServiceCommand : IRequest<ApiResponse>
{
    public Guid BoatyardServiceId { get; set; }
    public string? TypeService { get; set; }
    public decimal? Price { get; set; }
    public bool? IsActive { get; set; }
}

public class UpdateBoatyardServiceRequest
{
    public string? TypeService { get; set; }
    public decimal? Price { get; set; }
    public bool? IsActive { get; set; }
}