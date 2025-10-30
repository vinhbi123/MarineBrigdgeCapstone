using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Common.Validators;
using ShipCapstone.Application.Features.Boatyards.Command.CreateBoatyard;
using ShipCapstone.Application.Features.Boatyards.Command.UpdateBoatyard;
using ShipCapstone.Application.Features.Boatyards.Query.GetBoatyardById;
using ShipCapstone.Application.Features.Boatyards.Query.GetBoatyardDetail;
using ShipCapstone.Application.Features.Boatyards.Query.GetBoatyards;
using ShipCapstone.Application.Features.BoatyardServices.Query.GetBoatyardServicesByBoatyardId;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Boatyards;
using ShipCapstone.Domain.Models.BoatyardServices;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Paginate.Interface;

namespace ShipCapstone.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.Boatyards.BoatyardEndpoint)]
public class BoatyardController : BaseController<BoatyardController>
{
    public BoatyardController(ILogger logger, IMediator mediator) : base(logger, mediator)
    {
    }

    [HttpPost(ApiEndPointConstant.Boatyards.BoatyardEndpoint)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateBoatyard([FromForm] CreateBoatyardCommand command,
        [FromServices] ValidationUtil<CreateBoatyardCommand> validationUtil)
    {
        var (isValid, response) = await validationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }

        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateBoatyard), apiResponse);
    }

    [HttpGet(ApiEndPointConstant.Boatyards.BoatyardEndpoint)]
    [ProducesResponseType<ApiResponse<IPaginate<GetBoatyardsResponse>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBoatyards([FromQuery] int page = 1, [FromQuery] int size = 30,
        [FromQuery] string? sortBy = null, [FromQuery] bool isAsc = false, [FromQuery] string? name = null)
    {
        var query = new GetBoatyardsQuery()
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

    [HttpGet(ApiEndPointConstant.Boatyards.BoatyardById)]
    [ProducesResponseType<ApiResponse<GetBoatyardByIdResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBoatyardById([FromRoute] Guid id)
    {
        var query = new GetBoatyardByIdQuery()
        {
            BoatyardId = id
        };

        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }

    [CustomAuthorize(ERole.Boatyard)]
    [HttpGet(ApiEndPointConstant.Boatyards.BoatyardDetail)]
    [ProducesResponseType<GetBoatyardDetailResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBoatyardDetail()
    {
        var query = new GetBoatyardDetailQuery();

        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }
    [CustomAuthorize(ERole.Boatyard)]
    [HttpPatch(ApiEndPointConstant.Boatyards.BoatyardEndpoint)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBoatyard([FromForm] UpdateBoatyardCommand command,
        [FromServices] ValidationUtil<UpdateBoatyardCommand> validationUtil)
    {
        var (isValid, response) = await validationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }

        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }
    [HttpGet(ApiEndPointConstant.Boatyards.BoatyardWithBoatyardServices)]
    [ProducesResponseType<ApiResponse<GetBoatyardServicesByBoatyardIdResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBoatyardServicesByBoatyardId([FromRoute] Guid id, [FromQuery] int page = 1,
        [FromQuery] int size = 30, string? sortBy = null, bool isAsc = false, string? name = null)
    {
        var query = new GetBoatyardServicesByBoatyardIdQuery
        {
            BoatyardId = id,
            Page = page,
            Size = size,
            SortBy = sortBy,
            IsAsc = isAsc,
            Name = name
        };

        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }

}