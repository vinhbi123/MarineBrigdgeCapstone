using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Common.Validators;
using ShipCapstone.Application.Features.DockSlots.Command.CreateDockSlot;
using ShipCapstone.Application.Features.DockSlots.Command.UpdateDockSlot;
using ShipCapstone.Application.Features.DockSlots.Query.GetDockSlotById;
using ShipCapstone.Application.Features.DockSlots.Query.GetDockSlots;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.DockSlots;
using ShipCapstone.Infrastructure.Paginate.Interface;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.DockSlots.DockSlotEndpoint)]
public class DockSlotController : BaseController<DockSlotController>
{
    public DockSlotController(ILogger logger, IMediator mediator) : base(logger, mediator)
    {
    }
    
    [CustomAuthorize(ERole.Boatyard)]
    [HttpPost(ApiEndPointConstant.DockSlots.DockSlotEndpoint)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateDockSlot([FromBody] CreateDockSlotCommand command,
        [FromServices] ValidationUtil<CreateDockSlotCommand> validationUtil)
    {
        var (isValid, response) = await validationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }

        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateDockSlot), apiResponse);
    }
    
    [CustomAuthorize(ERole.Boatyard)]
    [HttpPut(ApiEndPointConstant.DockSlots.DockSlotById)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateDockSlot([FromRoute] Guid id, [FromBody] UpdateDockSlotRequest request,
        [FromServices] ValidationUtil<UpdateDockSlotCommand> validationUtil)
    {
        var command = new UpdateDockSlotCommand()
        {
            DockSlotId = id,
            Name = request.Name,
            AssignedFrom = request.AssignedFrom,
            AssignedUntil = request.AssignedUntil,
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
    
    [CustomAuthorize(ERole.Boatyard)]
    [HttpGet(ApiEndPointConstant.DockSlots.DockSlotEndpoint)]
    [ProducesResponseType<ApiResponse<IPaginate<GetDockSlotsResponse>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDockSlots([FromQuery] int page = 1, [FromQuery] int size = 30,
        [FromQuery] string? sortBy = null, [FromQuery] bool isAsc = false, [FromQuery] string? name = null)
    {
        var query = new GetDockSlotsQuery()
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

    [CustomAuthorize(ERole.Boatyard)]
    [HttpGet(ApiEndPointConstant.DockSlots.DockSlotById)]
    [ProducesResponseType<ApiResponse<GetDockSlotByIdResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDockSlotById([FromRoute] Guid id)
    {
        var query = new GetDockSlotByIdQuery()
        {
            DockSlotId = id
        };

        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }
}