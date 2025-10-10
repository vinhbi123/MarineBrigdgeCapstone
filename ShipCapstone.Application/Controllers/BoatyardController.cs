using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Features.Boatyards.Command.CreateBoatyard;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Models.Common;

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
}