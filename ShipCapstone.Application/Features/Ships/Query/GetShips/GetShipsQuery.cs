using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Ships.Query.GetShips;

public class GetShipsQuery : IRequest<ApiResponse>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public string? SortBy { get; set; }
    public bool IsAsc { get; set; }
    public string? Name { get; set; }
}