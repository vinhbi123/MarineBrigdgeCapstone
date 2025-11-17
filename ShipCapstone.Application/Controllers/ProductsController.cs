using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Common.Validators;
using ShipCapstone.Application.Features.Products.Command.CreateProduct;
using ShipCapstone.Application.Features.Products.Command.UpdateProduct;
using ShipCapstone.Application.Features.Products.Query.GetProductById;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Products;

namespace ShipCapstone.Application.Controllers;

[ApiController]
[Route("api/v1/products")]
public class ProductsController : BaseController<ProductsController>
{
    public ProductsController(ILogger logger, IMediator mediator) : base(logger, mediator) { }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetProductResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductById([FromRoute] Guid id)
    {
        var query = new GetProductByIdQuery { ProductId = id };
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }
    /// <summary>
    /// Create new product
    /// POST /api/v1/products
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command,
        [FromServices] ValidationUtil<CreateProductCommand> validationUtil)
    {
        var (isValid, response) = await validationUtil.ValidateAsync(command);
        if (!isValid) return BadRequest(response);

        var apiResponse = await _mediator.Send(command);

        if (apiResponse?.Data is Guid createdId)
        {
            return CreatedAtAction(nameof(GetProductById), new { id = createdId }, apiResponse);
        }

        return StatusCode(StatusCodes.Status201Created, apiResponse);
    }
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id,
     [FromBody] UpdateProductCommand command,
     [FromServices] ValidationUtil<UpdateProductCommand> validationUtil)
    {
        command.ProductId = id;
        var (isValid, response) = await validationUtil.ValidateAsync(command);
        if (!isValid) return BadRequest(response);

        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }
}