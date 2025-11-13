using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Common.Validators;
using ShipCapstone.Application.Features.Reports.Command.CreateReportProblem;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.ReportProblemCommand;

namespace ShipCapstone.Application.Controllers
{
    [ApiController]
    [Route("api/v1/reports")]
    public class ReportsController : BaseController<ReportsController>
    {
        public ReportsController(ILogger logger, IMediator mediator) : base(logger, mediator) { }

        [HttpPost]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportProblemCommand command,
            [FromServices] ValidationUtil<CreateReportProblemCommand> validationUtil)
        {
            var (isValid, response) = await validationUtil.ValidateAsync(command);
            if (!isValid)
                return BadRequest(response);

            var apiResponse = await _mediator.Send(command);
            return Created("", apiResponse); 
        }
    }
}
