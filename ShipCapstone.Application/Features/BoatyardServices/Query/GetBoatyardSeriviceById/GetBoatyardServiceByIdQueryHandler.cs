using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.BoatyardServices;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.BoatyardServices.Query.GetBoatyardSeriviceById;

public class GetBoatyardServiceByIdQueryHandler : IRequestHandler<GetBoatyardServiceByIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;

    public GetBoatyardServiceByIdQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async ValueTask<ApiResponse> Handle(GetBoatyardServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var boatyardService = await _unitOfWork.GetRepository<BoatyardService>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.BoatyardServiceId
        );
        
        if(boatyardService == null)
        {
            throw new NotFoundException("Không tìm thấy dịch vụ xưởng");
        }
        
        var response = new GetBoatyardServiceByIdResponse()
        {
            Id = boatyardService.Id,
            TypeService = boatyardService.TypeService,
            Price = boatyardService.Price,
            IsActive = boatyardService.IsActive,
            CreatedDate = boatyardService.CreatedDate,
            LastModifiedDate = boatyardService.LastModifiedDate
        };
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy dịch vụ xưởng thành công",
            Data = response
        };
    }
}