using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Common.Validators;
using ShipCapstone.Application.Features.ModifierOptions.Command.CreateModifierOption;
using ShipCapstone.Application.Features.ModifierOptions.Command.UpdateModifierOption;
using ShipCapstone.Application.Features.ModifierOptions.Query.GetModifierOptionById;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.ModifierOptions;

namespace ShipCapstone.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.ModifierOptions.ModifierOptionsEndpoint)]
public class ModifierOptionController : BaseController<ModifierOptionController>
{
    private readonly ValidationUtil<UpdateModifierOptionCommand> _updateModifierOptionValidationUtil;
    
    public ModifierOptionController(ILogger logger, IMediator mediator,
        ValidationUtil<UpdateModifierOptionCommand> updateModifierOptionValidationUtil) : base(logger, mediator)
    {
        _updateModifierOptionValidationUtil = updateModifierOptionValidationUtil ?? throw new ArgumentNullException(nameof(updateModifierOptionValidationUtil));
    }
    
    [CustomAuthorize(ERole.Supplier)]
    [HttpPatch(ApiEndPointConstant.ModifierOptions.ModifierOptionById)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateModifierOption([FromRoute] Guid id, [FromBody] UpdateModifierOptionRequest request)
    {
        var command = new UpdateModifierOptionCommand
        {
            ModifierOptionId = id,
            Name = request.Name,
            DisplayOrder = request.DisplayOrder
        };
        
        var (isValid, response) = await _updateModifierOptionValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }

        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }

    [CustomAuthorize(ERole.Supplier)]
    [HttpGet(ApiEndPointConstant.ModifierOptions.ModifierOptionById)]
    [ProducesResponseType<ApiResponse<GetModifierOptionByIdResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetModifierOptionById([FromRoute] Guid id)
    {
        var query = new GetModifierOptionByIdQuery()
        {
            ModifierOptionId = id
        };
        
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }
}