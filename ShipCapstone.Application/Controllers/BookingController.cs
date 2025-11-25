using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Domain.Models.Booking;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Application.Features.Bookings.Command.CreateBooking;
using ShipCapstone.Domain.Constants;

namespace ShipCapstone.Application.Controllers
{
     public class BookingController : BaseController<BookingController>
    {
        private readonly ValidationUtil<CreateBookingCommand> _createBookingValidationUtil;
        public BookingController(ILogger logger, IMediator mediator, ValidationUtil<CreateBookingCommand> createOrderValidationUtil) : base(logger, mediator)
        {
            _createBookingValidationUtil = createOrderValidationUtil;
        }

        [HttpPost(ApiEndPointConstant.Booking.CreateBooking)]
        [ProducesResponseType<ApiResponse<CreateBookingResponse>>(StatusCodes.Status201Created)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateBookingCommand command)
        {
            var (isValid, response) = await _createBookingValidationUtil.ValidateAsync(command);
            if (!isValid)
            {
                return BadRequest(response);
            }

            var apiResponse = await _mediator.Send(command);
            return CreatedAtAction(nameof(Create), apiResponse);
        }
    }
}
