using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Features.Revenues.Command.CreateUrlPaymentRevenue;
using ShipCapstone.Application.Features.Revenues.Query.GetRevenue;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Revenues;

namespace ShipCapstone.Application.Controllers;

public class RevenueController : BaseController<RevenueController>
{
    public RevenueController(ILogger logger, IMediator mediator) : base(logger, mediator)
    {
    }
    
    [HttpPost(ApiEndPointConstant.Revenue.RevenueEndPoint)]
    [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateServiceAppointment([FromBody] CreateUrlPaymentRevenueCommand command)
    {
        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }
    
    [HttpGet(ApiEndPointConstant.Revenue.RevenueEndPoint)]
    [ProducesResponseType<ApiResponse<List<GetRevenueResponse>>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRevenue([FromQuery] DateOnly? startDate, [FromQuery] DateOnly? endDate)
    {
        var query = new GetRevenueQuery()
        {
            StartDate = startDate,
            EndDate = endDate,
        };
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }
}