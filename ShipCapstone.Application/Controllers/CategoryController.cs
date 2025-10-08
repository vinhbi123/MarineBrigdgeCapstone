using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Common.Validators;
using ShipCapstone.Application.Features.Categories.Command.CreateCategory;
using ShipCapstone.Application.Features.Categories.Command.UpdateCategory;
using ShipCapstone.Application.Features.Categories.Query.GetCategories;
using ShipCapstone.Application.Features.Categories.Query.GetCategoryById;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Categories;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Paginate.Interface;

namespace ShipCapstone.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.Categories.CategoryEndpoint)]
public class CategoryController : BaseController<CategoryController>
{
    private readonly ValidationUtil<CreateCategoryCommand> _createCategoryValidationUtil;
    private readonly ValidationUtil<UpdateCategoryCommand> _updateCategoryValidationUtil;
    
    public CategoryController(ILogger logger, IMediator mediator,
        ValidationUtil<CreateCategoryCommand> createCategoryValidationUtil,
        ValidationUtil<UpdateCategoryCommand> updateCategoryValidationUtil) : base(logger, mediator)
    {
        _createCategoryValidationUtil = createCategoryValidationUtil ?? throw new ArgumentNullException(nameof(createCategoryValidationUtil));
        _updateCategoryValidationUtil = updateCategoryValidationUtil ?? throw new ArgumentNullException(nameof(updateCategoryValidationUtil));
    }

    [CustomAuthorize(ERole.Supplier)]
    [HttpPost(ApiEndPointConstant.Categories.CategoryEndpoint)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryCommand command)
    {
        var (isValid, response) = await _createCategoryValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }

        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateCategory), apiResponse);
    }

    [CustomAuthorize(ERole.Supplier)]
    [HttpPatch(ApiEndPointConstant.Categories.CategoryById)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromForm] UpdateCategoryRequest request)
    {
        var command = new UpdateCategoryCommand
        {
            CategoryId = id,
            Name = request.Name,
            Description = request.Description,
            Image = request.Image,
            IsActive = request.IsActive
        };
        
        var (isValid, response) = await _updateCategoryValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }

        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }
    [CustomAuthorize(ERole.Supplier)]
    [HttpGet(ApiEndPointConstant.Categories.CategoryEndpoint)]
    [ProducesResponseType<ApiResponse<IPaginate<GetCategoriesResponse>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCategories([FromQuery] int page = 1, [FromQuery] int size = 30,
        [FromQuery] string? sortBy = null, [FromQuery] bool isAsc = false, [FromQuery] string? name = null)
    {
        var query = new GetCategoriesQuery() 
        {
            Page = page,
            Size = size,
            SortBy = sortBy,
            IsAsc = isAsc,
            Name = name
        };
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }

    [CustomAuthorize(ERole.Supplier)]
    [HttpGet(ApiEndPointConstant.Categories.CategoryById)]
    [ProducesResponseType<ApiResponse<GetCategoryByIdResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
    {
        var query = new GetCategoryByIdQuery()
        {
            CategoryId = id
        };
        
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }
}