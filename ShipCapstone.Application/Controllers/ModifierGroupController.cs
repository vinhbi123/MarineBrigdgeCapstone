using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Common.Validators;
using ShipCapstone.Application.Features.ModifierGroups.Command.CreateModifierGroup;
using ShipCapstone.Application.Features.ModifierGroups.Command.UpdateModifierGroup;
using ShipCapstone.Application.Features.ModifierGroups.Query.GetModifierGroupById;
using ShipCapstone.Application.Features.ModifierGroups.Query.GetModifierGroups;
using ShipCapstone.Application.Features.ModifierOptions.Command.CreateModifierOption;
using ShipCapstone.Application.Features.ModifierOptions.Query.GetModifierOptionsByGroupId;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.ModifierGroups;
using ShipCapstone.Domain.Models.ModifierOptions;
using ShipCapstone.Infrastructure.Paginate.Interface;
using CreateModifierOptionRequest = ShipCapstone.Application.Features.ModifierGroups.Command.CreateModifierGroup.CreateModifierOptionRequest;

namespace ShipCapstone.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.ModifierGroups.ModifierGroupEndpoint)]
public class ModifierGroupController : BaseController<ModifierGroupController>
{
    private readonly ValidationUtil<CreateModifierGroupCommand> _createModifierGroupValidationUtil;
    private readonly ValidationUtil<UpdateModifierGroupCommand> _updateModifierGroupValidationUtil;
    private readonly ValidationUtil<CreateModifierOptionCommand> _createModifierOptionValidationUtil;
    
    
    public ModifierGroupController(ILogger logger, IMediator mediator,
        ValidationUtil<CreateModifierGroupCommand> createModifierGroupValidationUtil,
        ValidationUtil<UpdateModifierGroupCommand> updateModifierGroupValidationUtil,
        ValidationUtil<CreateModifierOptionCommand> createModifierOptionValidationUtil) : base(logger, mediator)
    {
        _createModifierGroupValidationUtil = createModifierGroupValidationUtil ?? throw new ArgumentNullException(nameof(createModifierGroupValidationUtil));
        _updateModifierGroupValidationUtil = updateModifierGroupValidationUtil ?? throw new ArgumentNullException(nameof(updateModifierGroupValidationUtil));
        _createModifierOptionValidationUtil = createModifierOptionValidationUtil ?? throw new ArgumentNullException(nameof(createModifierOptionValidationUtil));
    }

    [CustomAuthorize(ERole.Supplier)]
    [HttpPost(ApiEndPointConstant.ModifierGroups.ModifierGroupEndpoint)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateModifierGroup([FromBody] CreateModifierGroupCommand command)
    {
        var (isValid, response) = await _createModifierGroupValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }

        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateModifierGroup), apiResponse);
    }
    
    [CustomAuthorize(ERole.Supplier)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    [HttpPut(ApiEndPointConstant.ModifierGroups.ModifierGroupById)]
    public async Task<IActionResult> UpdateModifierGroup([FromRoute] Guid id, [FromBody] UpdateModifierGroupRequest request)
    {
        var command = new UpdateModifierGroupCommand()
        {
            ModifierGroupId = id,
            Name = request.Name
        };
        var (isValid, response) = await _updateModifierGroupValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }

        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }

    [CustomAuthorize(ERole.Supplier)]
    [HttpGet(ApiEndPointConstant.ModifierGroups.ModifierGroupEndpoint)]
    [ProducesResponseType<ApiResponse<IPaginate<GetModifierGroupsResponse>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetModifierGroups([FromQuery] int page = 1, [FromQuery] int size = 30,
        [FromQuery] string? sortBy = null, [FromQuery] bool isAsc = false, [FromQuery] string? name = null)
    {
        var query = new GetModifierGroupsQuery()
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

    [HttpGet(ApiEndPointConstant.ModifierGroups.ModifierGroupById)]
    [ProducesResponseType<ApiResponse<GetModifierGroupByIdResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetModifierGroupById([FromRoute] Guid id)
    {
        var query = new GetModifierGroupByIdQuery()
        {
            ModifierGroupId = id
        };
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }
    
    [CustomAuthorize(ERole.Supplier)]
    [HttpGet(ApiEndPointConstant.ModifierGroups.ModifierGroupByIdWithOptions)]
    [ProducesResponseType<ApiResponse<GetModifierOptionsByGroupIdResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetModifierOptionsByGroupId([FromRoute] Guid id)
    {
        var query = new GetModifierOptionsByGroupIdQuery()
        {
            ModifierGroupId = id
        };
        
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }
    [CustomAuthorize(ERole.Supplier)]
    [HttpPost(ApiEndPointConstant.ModifierGroups.ModifierGroupByIdWithOptions)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateModifierOption([FromRoute] Guid id, [FromBody] CreateModifierOptionRequest request)
    {
        var command = new CreateModifierOptionCommand()
        {
            ModifierGroupId = id,
            Name = request.Name,
            DisplayOrder = request.DisplayOrder
        };
        
        var (isValid, response) = await _createModifierOptionValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }

        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateModifierOption), apiResponse);
    }
    
}