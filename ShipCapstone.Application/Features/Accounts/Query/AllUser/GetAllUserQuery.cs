using Mediator;
using ShipCapstone.Domain.Models.Common;
namespace ShipCapstone.Application.Features.Accounts.Query.AllUser
{
    public class GetAllUserQuery : IRequest<ApiResponse>
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public string? SortBy { get; set; }
        public bool IsAsc { get; set; }
        public string? Name { get; set; }
    }
}