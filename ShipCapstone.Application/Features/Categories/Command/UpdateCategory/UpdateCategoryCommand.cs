using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Categories.Command.UpdateCategory;

public class UpdateCategoryCommand : IRequest<ApiResponse>
{
    public Guid CategoryId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IFormFile? Image { get; set; }
    public bool? IsActive { get; set; }
}

public class UpdateCategoryRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IFormFile? Image { get; set; }
    public bool? IsActive { get; set; }
}