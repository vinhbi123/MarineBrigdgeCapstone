using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Ports;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Ports.Query.GetPortById;

public class GetPortByIdQueryHandler : IRequestHandler<GetPortByIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    
    public GetPortByIdQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async ValueTask<ApiResponse> Handle(GetPortByIdQuery request, CancellationToken cancellationToken)
    {
        var port = await _unitOfWork.GetRepository<Port>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.PortId
        );
        if (port == null)
        {
            throw new NotFoundException("Cảng không tồn tại");
        }

        var response = new GetPortByIdResponse()
        {
            Id = port.Id,
            Name = port.Name,
            Country = port.Country,
            City = port.City,
            Longitude = port.Longitude,
            Latitude = port.Latitude,
            CreatedDate = port.CreatedDate,
            LastModifiedDate = port.LastModifiedDate
        };

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy cảng thành công",
            Data = response
        };
    }
}