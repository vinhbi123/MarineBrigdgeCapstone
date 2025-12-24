using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Validators;
using ShipCapstone.Application.Features.ProductVariantOptions.Command.AddProductVariantOption;
using ShipCapstone.Application.Features.ProductVariantOptions.Command.RemoveProductVariantOption;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.ProductVariantOptions.ProductVariantOptionEndpoint)]
public class ProductVariantOptionController : BaseController<ProductVariantOptionController>
{
    public ProductVariantOptionController(ILogger logger, IMediator mediator) : base(logger, mediator)
    {
    }
    
    [CustomAuthorize(ERole.Supplier)]
    [HttpDelete(ApiEndPointConstant.ProductVariantOptions.ProductVariantOptionById)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveProductVariantOption([FromRoute] Guid id)
    {
        var command = new RemoveProductVariantOptionCommand()
        {
            ProductVariantOptionId = id
        };
        
        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }
    
    [CustomAuthorize(ERole.Supplier)]
    [HttpPost(ApiEndPointConstant.ProductVariantOptions.ProductVariantOptionEndpoint)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddProductVariantOption([FromBody] AddProductVariantOptionCommand command)
    {
        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(AddProductVariantOption), apiResponse);
    }
}