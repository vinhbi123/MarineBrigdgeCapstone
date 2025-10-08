using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Categories.Command.CreateCategory;

public class CreateCategoryCommand : IRequest<ApiResponse>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public IFormFile? Image { get; set; }
}