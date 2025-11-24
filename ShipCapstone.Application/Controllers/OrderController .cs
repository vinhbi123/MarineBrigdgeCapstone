using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Features.Authentication.Command.SendOtp;
using ShipCapstone.Application.Features.Orders.Command.CreateOrder;
using ShipCapstone.Application.Features.Orders.Query;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Orders;

namespace ShipCapstone.Application.Controllers
{
    public class OrderController : BaseController<OrderController>
    {
        private readonly ValidationUtil<CreateOrderCommand> _createOrderValidationUtil;
        public OrderController(ILogger logger, IMediator mediator, ValidationUtil<CreateOrderCommand> createOrderValidationUtil) : base(logger, mediator)
        {
            _createOrderValidationUtil = createOrderValidationUtil;
        }

        [HttpPost(ApiEndPointConstant.Order.OrderEndpoint)]
        [ProducesResponseType<ApiResponse<CreateOrderResponse>>(StatusCodes.Status201Created)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> SendOtp([FromBody] CreateOrderCommand command)
        {
            var (isValid, response) = await _createOrderValidationUtil.ValidateAsync(command);
            if (!isValid)
            {
                return BadRequest(response);
            }

            var apiResponse = await _mediator.Send(command);
            return CreatedAtAction(nameof(SendOtp), apiResponse);
        }
    
     [HttpGet(ApiEndPointConstant.Order.OrderEndpoint + "/{id}")]
        [ProducesResponseType<ApiResponse<GetOrderResponse>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var query = new GetOrderByIdQuery { Id = id };
            var apiResponse = await _mediator.Send(query);
            return Ok(apiResponse);
        }
    }
}

