using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Ports.Command.UpdatePort;

public class UpdatePortCommandHandler : IRequestHandler<UpdatePortCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    
    public UpdatePortCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask<ApiResponse> Handle(UpdatePortCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("Handling UpdatePortCommand: {@Request}", request);
        var port = await _unitOfWork.GetRepository<Port>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.PortId
        );

        if (port == null)
        {
            throw new NotFoundException("Cảng không tồn tại");
        }

        port.Name = request.Name ?? port.Name;
        port.Country = request.Country ?? port.Country;
        port.City = request.City ?? port.City;
        port.Longitude = request.Longitude ?? port.Longitude;
        port.Latitude = request.Latitude ?? port.Latitude;

        _unitOfWork.GetRepository<Port>().UpdateAsync(port);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;

        if (!isSuccess)
        {
            _logger.Error("Cập nhật cảng thất bại: {@PortId}", port.Id);
            throw new Exception("Cập nhật cảng thất bại");
        }
        _logger.Information("Cập nhật cảng thành công: {@PortId}", port.Id);
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Cập nhật cảng thành công",
            Data = port.Id
        };
    }
}