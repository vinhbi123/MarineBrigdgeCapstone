using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Common.Validators;
using ShipCapstone.Application.Features.Ships.Command.CreateShip;
using ShipCapstone.Application.Features.Ships.Command.UpdateShip;
using ShipCapstone.Application.Features.Ships.Query.GetShipById;
using ShipCapstone.Application.Features.Ships.Query.GetShips;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Ships;
using ShipCapstone.Infrastructure.Paginate.Interface;

namespace ShipCapstone.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.Ships.ShipEndpoint)]
public class ShipController : BaseController<ShipController>
{
    private readonly ValidationUtil<CreateShipCommand> _createShipValidationUtil;
    private readonly ValidationUtil<UpdateShipCommand> _updateShipValidationUtil;
    
    public ShipController(ILogger logger, IMediator mediator,
        ValidationUtil<CreateShipCommand> createShipValidationUtil,
        ValidationUtil<UpdateShipCommand> updateShipValidationUtil) : base(logger, mediator)
    {
        _createShipValidationUtil = createShipValidationUtil ?? throw new ArgumentNullException(nameof(createShipValidationUtil));
        _updateShipValidationUtil = updateShipValidationUtil ?? throw new ArgumentNullException(nameof(updateShipValidationUtil));
    }

    [CustomAuthorize(ERole.User)]
    [HttpPost(ApiEndPointConstant.Ships.ShipEndpoint)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateShip([FromBody] CreateShipCommand command)
    {
        var (isValid, response) = await _createShipValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }
        
        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateShip), apiResponse);
    }

    [CustomAuthorize(ERole.User)]
    [HttpPatch(ApiEndPointConstant.Ships.ShipById)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateShip([FromRoute] Guid id, [FromBody] UpdateShipRequest request)
    {
        var command = new UpdateShipCommand
        {
            ShipId = id,
            Name = request.Name,
            ImoNumber = request.ImoNumber,
            RegisterNo = request.RegisterNo,
            BuildYear = request.BuildYear,
            Longitude = request.Longitude,
            Latitude = request.Latitude
        };
        var (isValid, response) = await _updateShipValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }
        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }
    [CustomAuthorize(ERole.User)]
    [HttpGet(ApiEndPointConstant.Ships.ShipEndpoint)]
    [ProducesResponseType<ApiResponse<IPaginate<GetShipsResponse>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetShips([FromQuery] int page = 1, [FromQuery] int size = 30,
        [FromQuery] string? sortBy = null, [FromQuery] bool isAsc = false, [FromQuery] string? name = null)
    {
        var query = new GetShipsQuery()
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

    [CustomAuthorize(ERole.User)]
    [HttpGet(ApiEndPointConstant.Ships.ShipById)]
    [ProducesResponseType<ApiResponse<GetShipByIdResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetShipById([FromRoute] Guid id)
    {
        var query = new GetShipByIdQuery()
        {
            ShipId = id
        };
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }
}