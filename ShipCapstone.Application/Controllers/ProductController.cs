using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Common.Validators;
using ShipCapstone.Application.Features.Products.Command.CreateProduct;
using ShipCapstone.Application.Features.Products.Command.UpdateProduct;
using ShipCapstone.Application.Features.Products.Query.GetProductById;
using ShipCapstone.Application.Features.Products.Query.GetProducts;
using ShipCapstone.Application.Features.ProductVariants.Command.CreateProductVariant;
using ShipCapstone.Application.Features.ProductVariants.Command.UpdateProductVariant;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Products;
using ShipCapstone.Infrastructure.Paginate.Interface;

namespace ShipCapstone.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.Products.ProductEndpoint)]
public class ProductController : BaseController<ProductController>
{
    public ProductController(ILogger logger, IMediator mediator) : base(logger, mediator)
    {
    }

    [CustomAuthorize(ERole.Supplier)]
    [HttpPost(ApiEndPointConstant.Products.ProductEndpoint)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProduct([FromForm] CreateProductCommand command,
        [FromServices] ValidationUtil<CreateProductCommand> validationUtil)
    {
        var (isValid, response) = await validationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }

        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateProduct), apiResponse);
    }

    [HttpGet(ApiEndPointConstant.Products.ProductEndpoint)]
    [ProducesResponseType<ApiResponse<IPaginate<GetProductsResponse>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProducts([FromQuery] int page = 1, [FromQuery] int size = 10,
        [FromQuery] string? name = null, [FromQuery] string? sortBy = null, [FromQuery] bool isAsc = false)
    {
        var query = new GetProductsQuery()
        {
            Page = page,
            Size = size,
            Name = name,
            SortBy = sortBy,
            IsAsc = isAsc
        };
        
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }

    [HttpGet(ApiEndPointConstant.Products.ProductById)]
    [ProducesResponseType<ApiResponse<GetProductByIdResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductById([FromRoute] Guid id)
    {
        var query = new GetProductByIdQuery()
        {
            ProductId = id
        };
        
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }

    [CustomAuthorize(ERole.Supplier)]
    [HttpPatch(ApiEndPointConstant.Products.ProductById)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromForm] UpdateProductRequest request,
        [FromServices] ValidationUtil<UpdateProductCommand> validationUtil)
    {
        var command = new UpdateProductCommand()
        {
            ProductId = id,
            Name = request.Name,
            Description = request.Description,
            CategoryId = request.CategoryId
        };
        
        var (isValid, response) = await validationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }
        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }

    [CustomAuthorize(ERole.Supplier)]
    [HttpPost(ApiEndPointConstant.Products.ProductWithVariants)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProductVariant([FromRoute] Guid id,
        [FromBody] CreateProductVariantRequest request, [FromServices] ValidationUtil<CreateProductVariantCommand> validationUtil)
    {
        var command = new CreateProductVariantCommand()
        {
            ProductId = id,
            Name = request.Name,
            Price = request.Price
        };
        
        var (isValid, response) = await validationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }
        
        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateProductVariant), apiResponse);
    }

}