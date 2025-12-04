using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Features.ReportProblems.Command.CreateReportProblem;
using ShipCapstone.Application.Features.ReportProblems.Query.GetReportById;
using ShipCapstone.Application.Features.ReportProblems.Query.GetReports;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.ReportProblems;

namespace ShipCapstone.Application.Controllers
{
    public class ReportProblemController : BaseController<ReportProblemController>
    {
        public ReportProblemController(ILogger logger, IMediator mediator) : base(logger, mediator) { }

        [HttpPost(ApiEndPointConstant.Report.ReportProblemEndpoint)]
        [ProducesResponseType<ApiResponse<ReportProblemResponse>>(StatusCodes.Status201Created)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateReportProblemCommand command)
        {
            var apiResponse = await _mediator.Send(command);
            return StatusCode(apiResponse.Status, apiResponse);
        }

        [HttpGet(ApiEndPointConstant.Report.ReportProblemEndpoint)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetReportsQuery query)
        {
            var apiResponse = await _mediator.Send(query);
            return Ok(apiResponse);
        }

        [HttpGet(ApiEndPointConstant.Report.ReportProblemEndpoint + "/{id:guid}")]
        [ProducesResponseType<ApiResponse<ReportProblemResponse>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var apiResponse = await _mediator.Send(new GetReportByIdQuery { Id = id });
            return Ok(apiResponse);
        }
    }
}
