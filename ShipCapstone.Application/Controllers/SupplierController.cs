using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Common.Validators;
using ShipCapstone.Application.Features.Suppliers.Command.CreateSupplier;
using ShipCapstone.Application.Features.Suppliers.Query.GetSuppliers;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Authentication;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Suppliers;
using ShipCapstone.Infrastructure.Paginate.Interface;

namespace ShipCapstone.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.Suppliers.SupplierEndpoint)]
public class SupplierController : BaseController<SupplierController>
{
    private ValidationUtil<CreateSupplierCommand> _createSupplierValidationUtil;
    public SupplierController(ILogger logger, IMediator mediator,
        ValidationUtil<CreateSupplierCommand> createSupplierValidationUtil) : base(logger, mediator)
    {
        _createSupplierValidationUtil = createSupplierValidationUtil ?? throw new ArgumentNullException(nameof(createSupplierValidationUtil));
    }

    [HttpPost(ApiEndPointConstant.Suppliers.SupplierEndpoint)]
    [ProducesResponseType<ApiResponse<LoginResponse>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateSupplier([FromForm] CreateSupplierCommand command)
    {
        var (isValid, response) = await _createSupplierValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }

        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateSupplier), apiResponse);
    }

    [CustomAuthorize(ERole.Admin)]
    [ProducesResponseType<ApiResponse<IPaginate<GetSuppliersResponse>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    [HttpGet(ApiEndPointConstant.Suppliers.SupplierEndpoint)]
    public async Task<IActionResult> GetSuppliers([FromQuery] int page = 1, [FromQuery] int size = 30,
        [FromQuery] string? sortBy = null, [FromQuery] bool isAsc = false, [FromQuery] string? name = null)
    {
        var query = new GetSuppliersQuery()
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