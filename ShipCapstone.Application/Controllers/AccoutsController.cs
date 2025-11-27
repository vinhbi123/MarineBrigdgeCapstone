using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Features.Accounts.Query.AllUser;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Profile;

namespace ShipCapstone.Application.Controllers
{
    public class AccoutController : BaseController<AccoutController>
    {
        public AccoutController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet(ApiEndPointConstant.Accouts.AccountEndpoint)]
        [ProducesResponseType<ApiResponse<ICollection<GetProfileResponse>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAccount([FromQuery] int page = 1, [FromQuery] int size = 30, [FromQuery] string? sortBy = null, [FromQuery] bool isAsc = false, [FromQuery] string? name = null)
        {
            var query = new GetAllUserQuery()
            {
                Page = page,
                Size = size,
                SortBy = sortBy,
                IsAsc = isAsc,
                Name = name
            };
            var apiResponse = await _mediator.Send(query);
            return Ok(apiResponse);
        }
    }
}
