using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Features.Payments.Command;
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
}