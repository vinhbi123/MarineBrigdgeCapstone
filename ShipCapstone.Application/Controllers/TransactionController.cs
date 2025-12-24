using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Features.Transactions.Query.GetTransactions;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Transactions;

namespace ShipCapstone.Application.Controllers;

public class TransactionController : BaseController<TransactionController>
{
    public TransactionController(ILogger logger, IMediator mediator) : base(logger, mediator)
    {
    }

    [HttpGet(ApiEndPointConstant.Transaction.TransactionEndPoint)]
    [ProducesResponseType<ApiResponse<ICollection<GetTransactionsResponse>>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRevenue([FromQuery] int page = 1, [FromQuery] int size = 30,
        [FromQuery] string? sortBy = null, [FromQuery] bool isAsc = false)
    {
        var query = new GetTransactionsQuery()
        {
            Page = page,
            Size = size,
            SortBy = sortBy,
            IsAsc = isAsc
        };
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }
}