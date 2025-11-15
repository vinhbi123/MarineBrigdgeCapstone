using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Common.Validators;
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
