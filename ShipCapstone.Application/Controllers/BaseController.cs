using Mediator;
using ShipCapstone.Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace ShipCapstone.Application.Controllers;

[Route(ApiEndPointConstant.ApiEndpoint)]
[ApiController]
public class BaseController<T> : ControllerBase where T : BaseController<T>
{
    protected ILogger _logger;
    protected IMediator _mediator;

    public BaseController(ILogger logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
}