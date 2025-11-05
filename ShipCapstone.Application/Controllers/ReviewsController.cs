using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Common.Validators;
using ShipCapstone.Application.Features.Review.Command.CreateReview;
using ShipCapstone.Application.Features.Review.Command.DeleteReview;
using ShipCapstone.Application.Features.Review.Query.GetReviewsByProduct;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Reviews;
using ShipCapstone.Infrastructure.Paginate.Interface;

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

        /// <summary>
        /// Lấy danh sách đánh giá theo productId (paging)
        /// GET /api/v1/reviews/product/{productId}?page=1&size=20&sortBy=&isAsc=false
        /// </summary>
        [HttpGet("product/{productId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<IPaginate<GetReviewResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReviewsByProduct([FromRoute] Guid productId,
            [FromQuery] int page = 1, [FromQuery] int size = 20,
            [FromQuery] string? sortBy = null, [FromQuery] bool isAsc = false)
        {
            var query = new GetReviewsByProductQuery()
            {
                ProductId = productId,
                Page = page,
                Size = size,
                SortBy = sortBy,
                IsAsc = isAsc
            };

            var apiResponse = await _mediator.Send(query);
            return Ok(apiResponse);
        }

        /// <summary>
        /// Xóa một đánh giá theo reviewId
        /// DELETE /api/v1/reviews/{id}
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReview([FromRoute] Guid id)
        {
            var command = new DeleteReviewCommand
            {
                ReviewId = id
            };

            var apiResponse = await _mediator.Send(command);
            return Ok(apiResponse);
        }
    }
}


 