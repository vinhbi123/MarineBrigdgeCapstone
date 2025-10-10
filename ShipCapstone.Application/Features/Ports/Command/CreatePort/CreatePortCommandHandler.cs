using Mediator;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Ports.Command.CreatePort;

public class CreatePortCommandHandler : IRequestHandler<CreatePortCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;

    public CreatePortCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async ValueTask<ApiResponse> Handle(CreatePortCommand request, CancellationToken cancellationToken)
    {
        var port = new Port()
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            Country = request.Country,
            City = request.City,
            Latitude = request.Latitude,
            Longitude = request.Longitude
        };

        await _unitOfWork.GetRepository<Port>().InsertAsync(port);

        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Thêm cảng thất bại");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Thêm cảng thành công",
            Data = port.Id
        };
    }
}