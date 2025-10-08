using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Features.Accounts.Command.Register;
using ShipCapstone.Application.Features.Authentication.Command.Login;
using ShipCapstone.Application.Features.Authentication.Command.Oauth;
using ShipCapstone.Application.Features.Authentication.Command.SendOtp;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Models.Authentication;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.Authentication.AuthenticationEndpoint)]
public class AuthenticationController : BaseController<AuthenticationController>
{
    private readonly ValidationUtil<LoginCommand> _loginValidationUtil;
    private readonly ValidationUtil<SendOtpCommand> _sendOtpValidationUtil;
    private readonly ValidationUtil<RegisterCommand> _registerValidationUtil;
    public AuthenticationController(ILogger logger, IMediator mediator,
        ValidationUtil<LoginCommand> loginValidationUtil,
        ValidationUtil<SendOtpCommand> sendOtpValidationUtil,
        ValidationUtil<RegisterCommand> registerValidationUtil) : base(logger, mediator)
    {
        _loginValidationUtil = loginValidationUtil ?? throw new ArgumentNullException(nameof(loginValidationUtil));
        _sendOtpValidationUtil = sendOtpValidationUtil ?? throw new ArgumentNullException(nameof(sendOtpValidationUtil));
        _registerValidationUtil = registerValidationUtil ?? throw new ArgumentNullException(nameof(registerValidationUtil));
    }

    [HttpPost(ApiEndPointConstant.Authentication.Login)]
    [ProducesResponseType<ApiResponse<LoginResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var (isValid, response) = await _loginValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }

        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }
    [HttpPost(ApiEndPointConstant.Authentication.Otp)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> SendOtp([FromBody] SendOtpCommand command)
    {
        var (isValid, response) = await _sendOtpValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }
        
        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(SendOtp), apiResponse);
    }
    
    [HttpPost(ApiEndPointConstant.Authentication.Register)]
    [ProducesResponseType<ApiResponse<LoginResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromForm] RegisterCommand command)
    {
        var (isValid, response) = await _registerValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }
        
        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }
    
    [HttpPost(ApiEndPointConstant.Authentication.OAuth)]
    [ProducesResponseType<ApiResponse<LoginResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> OAuth([FromBody] SignInGoogleCommand command)
    {
        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }
}