using Mediator;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Ports;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Ports.Query.GetPorts;

public class GetPortsQueryHandler : IRequestHandler<GetPortsQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    
    public GetPortsQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async ValueTask<ApiResponse> Handle(GetPortsQuery request, CancellationToken cancellationToken)
    {
        var ports = await _unitOfWork.GetRepository<Port>().GetPagingListAsync(
            selector: x => new GetPortsResponse()
            {
                Id = x.Id,
                Name = x.Name,
                Country = x.Country,
                City = x.City,
                Longitude = x.Longitude,
                Latitude = x.Latitude,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate
            },
            predicate: x => string.IsNullOrEmpty(request.Name) || x.Name.Contains(request.Name),
            page: request.Page,
            size: request.Size,
            sortBy: request.SortBy ?? nameof(Port.CreatedDate),
            isAsc: request.IsAsc
        );
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy danh sách cảng thành công",
            Data = ports
        };
    }
}