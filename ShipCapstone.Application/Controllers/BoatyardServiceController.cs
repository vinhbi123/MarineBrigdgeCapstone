using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Common.Validators;
using ShipCapstone.Application.Features.BoatyardServices.Command.CreateBoatyardService;
using ShipCapstone.Application.Features.BoatyardServices.Command.UpdateBoatyardService;
using ShipCapstone.Application.Features.BoatyardServices.Query.GetBoatyardSeriviceById;
using ShipCapstone.Application.Features.BoatyardServices.Query.GetBoatyardServices;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.BoatyardServices;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Paginate.Interface;

namespace ShipCapstone.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.BoatyardServices.BoatyardServiceEndpoint)]
public class BoatyardServiceController : BaseController<BoatyardController>
{
    public BoatyardServiceController(ILogger logger, IMediator mediator) : base(logger, mediator)
    {
    }

    [CustomAuthorize(ERole.Boatyard)]
    [HttpPost(ApiEndPointConstant.BoatyardServices.BoatyardServiceEndpoint)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateBoatyardService([FromBody] CreateBoatyardServiceCommand command,
        [FromServices] ValidationUtil<CreateBoatyardServiceCommand> validationUtil)
    {
        var (isValid, response) = await validationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }

        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateBoatyardService), apiResponse);
    }

    [CustomAuthorize(ERole.Boatyard)]
    [HttpPatch(ApiEndPointConstant.BoatyardServices.BoatyardServiceById)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBoatyardService([FromRoute] Guid id,
        [FromBody] UpdateBoatyardServiceRequest request,
        [FromServices] ValidationUtil<UpdateBoatyardServiceCommand> validationUtil)
    {
        var command = new UpdateBoatyardServiceCommand()
        {
            BoatyardServiceId = id,
            TypeService = request.TypeService,
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

    [CustomAuthorize(ERole.Boatyard)]
    [HttpGet(ApiEndPointConstant.BoatyardServices.BoatyardServiceEndpoint)]
    [ProducesResponseType<ApiResponse<IPaginate<GetBoatyardServicesResponse>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBoatyardServices([FromQuery] int page = 1, [FromQuery] int size = 30,
        [FromQuery] string? sortBy = null, [FromQuery] bool isAsc = false, [FromQuery] string? typeService = null)
    {
        var query = new GetBoatyardServicesQuery()
        {
            Page = page,
            Size = size,
            SortBy = sortBy,
            IsAsc = isAsc,
            TypeService = typeService
        };

        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }

    [CustomAuthorize(ERole.Boatyard)]
    [HttpGet(ApiEndPointConstant.BoatyardServices.BoatyardServiceById)]
    [ProducesResponseType<GetBoatyardServiceByIdResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBoatyardServicesForBoatyard([FromRoute] Guid id)
    {
        var query = new GetBoatyardServiceByIdQuery()
        {
            BoatyardServiceId = id
        };

        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }
}