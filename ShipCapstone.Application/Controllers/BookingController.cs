using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Features.Bookings.Command.CreateBooking;
using ShipCapstone.Application.Features.Bookings.Query.GetBooking;
using ShipCapstone.Application.Features.Bookings.Query.GetBookingById;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Models.Booking;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Paginate.Interface;

namespace ShipCapstone.Application.Controllers
{
     public class BookingController : BaseController<BookingController>
    {
        private readonly ValidationUtil<CreateBookingCommand> _createBookingValidationUtil;
        public BookingController(ILogger logger, IMediator mediator, ValidationUtil<CreateBookingCommand> createOrderValidationUtil) : base(logger, mediator)
        {
            _createBookingValidationUtil = createOrderValidationUtil;
        }

        [HttpPost(ApiEndPointConstant.Bookings.BookingEndPoint)]
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
        [HttpGet(ApiEndPointConstant.Bookings.BookingEndPoint)]
        [ProducesResponseType<ApiResponse<IPaginate<GetAllBookingResponse>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBooking([FromQuery] int page = 1, [FromQuery] int size = 30,
           [FromQuery] string? sortBy = null, [FromQuery] bool isAsc = false)
        {
            var query = new GetAllBookingQuery()
            {
                Page = page,
                Size = size,
                SortBy = sortBy,
                IsAsc = isAsc
            };

            var apiResponse = await _mediator.Send(query);
            return Ok(apiResponse);
        }

        [HttpGet(ApiEndPointConstant.Bookings.BookingById)]
        [ProducesResponseType<ApiResponse<GetBookingByIdResponse>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookingById([FromRoute] Guid id)
        {
            var query = new GetBookingByIdQuery()
            {
                Id = id
            };

            var apiResponse = await _mediator.Send(query);
            return Ok(apiResponse);
        }
    }
}
  