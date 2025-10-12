using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Boatyards.Query.GetBoatyards;

public class GetBoatyardsQuery : IRequest<ApiResponse>
{
    public int Size { get; set; }
    public int Page { get; set; }
    public string? SortBy { get; set; }
    public bool IsAsc { get; set; }
    public string? Name { get; set; }
}