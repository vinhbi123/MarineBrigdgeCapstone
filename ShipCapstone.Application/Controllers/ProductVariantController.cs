using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Common.Validators;
using ShipCapstone.Application.Features.ProductVariants.Command.UpdateProductVariant;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.ProductVariants.ProductVariantEndpoint)]
public class ProductVariantController : BaseController<ProductVariantController>
{
    public ProductVariantController(ILogger logger, IMediator mediator) : base(logger, mediator)
    {
    }

    [CustomAuthorize(ERole.Supplier)]
    [HttpPatch(ApiEndPointConstant.ProductVariants.ProductVariantById)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProductVariant([FromRoute] Guid id,
        [FromBody] UpdateProductVariantRequest request, 
        [FromServices] ValidationUtil<UpdateProductVariantCommand> validationUtil)
    {
        var command = new UpdateProductVariantCommand
        {
            ProductVariantId = id,
            Name = request.Name,
            Price = request.Price,
            IsActive = request.IsActive
        };
        
        var (isValid, response) = await validationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }
        
        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }

}