using Mediator;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Newtonsoft.Json;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Features.Payments.Command;
using ShipCapstone.Application.Features.Payments.Command.CreatePayment;
using ShipCapstone.Application.Features.Payments.Command.PaymentWebhook;
using ShipCapstone.Application.Features.Revenues.Command.HandleTransactionRevenue;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Controllers;

public class PaymentController : BaseController<PaymentController>
{
    private readonly ValidationUtil<CreatePaymentCommand> _createPaymentValidationUtil;
    public PaymentController(ILogger logger, IMediator mediator, ValidationUtil<CreatePaymentCommand> createPaymentValidationUtil) : base(logger, mediator)
    {
        _createPaymentValidationUtil = createPaymentValidationUtil ?? throw new ArgumentNullException(nameof(createPaymentValidationUtil));
    }
    
    [HttpPost(ApiEndPointConstant.Payments.PaymentEndpoint)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCategory([FromBody] CreatePaymentCommand command)
    {
        var (isValid, response) = await _createPaymentValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }

        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateCategory), apiResponse);
    }
    
    [HttpPost(ApiEndPointConstant.Payments.HandlePayment)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> HandlePayment([FromBody] WebhookType payload)
    {
        try
        {
            var command = new ConfirmWebhookCommand()
            {
                Payload = payload
            };
            var apiResponse = await _mediator.Send(command);
            return Ok(apiResponse);
        }
        catch (Exception ex)
        { return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the webhook.");
        }
    }
    
    [HttpPost(ApiEndPointConstant.Payments.HandlerPaymentSepay)]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> HandleRevenue([FromBody] HandleTransactionRevenueCommand command)
    {
        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }
}