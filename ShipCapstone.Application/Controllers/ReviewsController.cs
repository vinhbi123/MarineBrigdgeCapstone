using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Common.Validators;
using ShipCapstone.Application.Features.Reviews.Command.CreateReview;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Controllers
{
    [ApiController]
    [Route("api/v1/reviews")]
    public class ReviewsController : BaseController<ReviewsController>
    {
        public ReviewsController(ILogger logger, IMediator mediator) : base(logger, mediator) { }

        [HttpPost]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewCommand command,
            [FromServices] ValidationUtil<CreateReviewCommand> validationUtil)
        {
            var (isValid, response) = await validationUtil.ValidateAsync(command);
            if (!isValid)
            {
                return BadRequest(response);
            }

            var apiResponse = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateReview), apiResponse);
        }
    }
}
