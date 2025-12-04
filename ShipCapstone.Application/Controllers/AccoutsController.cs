using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Features.Accounts.Command.ChangePassword;
using ShipCapstone.Application.Features.Accounts.Query.AllUser;
using ShipCapstone.Domain.Constants;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Profile;

namespace ShipCapstone.Application.Controllers
{
    public class AccoutController : BaseController<AccoutController>
    {
        private readonly ValidationUtil<ChangePasswordCommand> _changePasswordValidationUtil;
        public AccoutController(ILogger logger, IMediator mediator, ValidationUtil<ChangePasswordCommand> changePasswordValidationUtil) : base(logger, mediator)
        {
            _changePasswordValidationUtil = changePasswordValidationUtil ?? throw new ArgumentNullException(nameof(changePasswordValidationUtil));
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

        [HttpPatch(ApiEndPointConstant.Accouts.ChangePassword)]
        [ProducesResponseType<ApiResponse<Guid>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var (isValid, response) = await _changePasswordValidationUtil.ValidateAsync(command);
            if (!isValid)
            {
                return BadRequest(response);
            }
            var apiResponse = await _mediator.Send(command);
            return Ok(apiResponse);
        }
    }
}
