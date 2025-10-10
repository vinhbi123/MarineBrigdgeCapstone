using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Common.Validators;
using ShipCapstone.Application.Features.Ports.Command.CreatePort;
using ShipCapstone.Application.Features.Ports.Command.UpdatePort;
using ShipCapstone.Application.Features.Ports.Query.GetPortById;
using ShipCapstone.Application.Features.Ports.Query.GetPorts;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Ports;
using ShipCapstone.Infrastructure.Paginate.Interface;

namespace ShipCapstone.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.Ports.PortEndpoint)]
public class PortController : BaseController<PortController>
{

    public PortController(ILogger logger, IMediator mediator) : base(logger, mediator)
    {
    }

    [CustomAuthorize(ERole.Admin)]
    [HttpPost(ApiEndPointConstant.Ports.PortEndpoint)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePort([FromBody] CreatePortCommand command,
        [FromServices] ValidationUtil<CreatePortCommand> validationUtil)
    {
        var (isValid, response) = await validationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }
        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreatePort), apiResponse);
    }

    [CustomAuthorize(ERole.Admin)]
    [HttpPatch(ApiEndPointConstant.Ports.PortById)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePort([FromRoute] Guid id, [FromBody] UpdatePortRequest request,
        [FromServices] ValidationUtil<UpdatePortCommand> validationUtil)
    {
        var command = new UpdatePortCommand()
        {
            PortId = id,
            Name = request.Name,
            Country = request.Country,
            City = request.City,
            Longitude = request.Longitude,
            Latitude = request.Latitude
        };
        var (isValid, response) = await validationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }
        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }

    [HttpGet(ApiEndPointConstant.Ports.PortEndpoint)]
    [ProducesResponseType<ApiResponse<IPaginate<GetPortsResponse>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPorts([FromQuery] int page = 1, [FromQuery] int size = 30,
        [FromQuery] string? sortBy = null, [FromQuery] bool isAsc = false, [FromQuery] string? name = null)
    {
        var query = new GetPortsQuery()
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

    [HttpGet(ApiEndPointConstant.Ports.PortById)]
    [ProducesResponseType<ApiResponse<GetPortByIdResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPortById([FromRoute] Guid id)
    {
        var query = new GetPortByIdQuery()
        {
            PortId = id
        };

        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }
}