using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Features.Captains.Command.RegisterCaptain;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Models.Authentication;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Controllers;

public class CaptainController : BaseController<CaptainController>
{
    private readonly ValidationUtil<RegisterCaptainCommand> _registerCaptainValidationUtil;
    public CaptainController(ILogger logger, IMediator mediator, 
        ValidationUtil<RegisterCaptainCommand> registerCaptainValidationUtil) : base(logger, mediator)
    {
        _registerCaptainValidationUtil = registerCaptainValidationUtil ?? throw new ArgumentNullException(nameof(registerCaptainValidationUtil));
    }
    
    [HttpPost(ApiEndPointConstant.Captains.CaptainEndpoint)]
    [ProducesResponseType<ApiResponse<LoginResponse>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterCaptain([FromForm] RegisterCaptainCommand command)
    {
        var (isValid, response) = await _registerCaptainValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }
        
        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(RegisterCaptain), apiResponse);
    }
}