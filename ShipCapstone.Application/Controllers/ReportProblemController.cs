using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Common.Validators;
using ShipCapstone.Application.Features.ReportProblems.Command.CreateReportProblem;
using ShipCapstone.Application.Features.ReportProblems.Command.DeleteReportProblem;
using ShipCapstone.Application.Features.ReportProblems.Command.UpdateReportProblem;
using ShipCapstone.Application.Features.ReportProblems.Query.GetAllReportProblemForCaptain;
using ShipCapstone.Application.Features.ReportProblems.Query.GetReportProblemById;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.ReportProblems;
using ShipCapstone.Infrastructure.Paginate.Interface;

namespace ShipCapstone.Application.Controllers;

public class ReportProblemController : BaseController<ReportProblemController>
{
    private readonly ValidationUtil<CreateReportProblemCommand> _createReportProblemValidationUtil;
    public ReportProblemController(ILogger logger, IMediator mediator, ValidationUtil<CreateReportProblemCommand> createReportProblemValidationUtil) : base(logger, mediator)
    {
        _createReportProblemValidationUtil = createReportProblemValidationUtil ?? throw new ArgumentNullException(nameof(createReportProblemValidationUtil));
    }
    
    [CustomAuthorize(ERole.Captain)]
    [HttpPost(ApiEndPointConstant.ReportProblems.ReportProblemEndpoint)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateReportProblem([FromBody] CreateReportProblemCommand command)
    {
        var (isValid, response) = await _createReportProblemValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }

        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateReportProblem), apiResponse);
    }
    
    [CustomAuthorize(ERole.Captain)]
    [HttpGet(ApiEndPointConstant.ReportProblems.ReportProblemEndpoint)]
    [ProducesResponseType<ApiResponse<IPaginate<GetAllReportProblemResponse>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllReportProblemByCaptain([FromQuery] int page = 1, [FromQuery] int size = 30,
        [FromQuery] string? sortBy = null, [FromQuery] bool isAsc = false)
    {
        var query = new GetAllReportProblemForCaptainQuery()
        {
            Page = page,
            Size = size,
            SortBy = sortBy,
            IsAsc = isAsc
        };
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }
    
    [CustomAuthorize(ERole.Captain, ERole.User)]
    [HttpGet(ApiEndPointConstant.ReportProblems.ReportProblemById)]
    [ProducesResponseType<ApiResponse<GetAllReportProblemResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReportProblemById([FromRoute] Guid id)
    {
        var query = new GetReportProblemByIdQuery()
        {
            Id = id
        };
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }
    
    [CustomAuthorize(ERole.User)]
    [HttpPatch(ApiEndPointConstant.ReportProblems.ReportProblemById)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateReportProblem([FromRoute] Guid id, [FromBody] UpdateReportProblemRequest request)
    {
        var command = new UpdateReportProblemCommand()
        {
            Id = id,
            Status = request.Status
        };
        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }
    
    [CustomAuthorize(ERole.Captain)]
    [HttpDelete(ApiEndPointConstant.ReportProblems.ReportProblemById)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteReportProblem([FromRoute] Guid id)
    {
        var command = new DeleteReportProblemCommand()
        {
            Id = id
        };
        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }
}