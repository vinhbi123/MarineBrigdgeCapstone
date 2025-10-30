using Mediator;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.BoatyardServices;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.BoatyardServices.Query.GetBoatyardServicesByBoatyardId;

public class GetBoatyardServicesByBoatyardIdQueryHandler : IRequestHandler<GetBoatyardServicesByBoatyardIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;

    public GetBoatyardServicesByBoatyardIdQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask<ApiResponse> Handle(GetBoatyardServicesByBoatyardIdQuery request, CancellationToken cancellationToken)
    {
        var boatyardServices = await _unitOfWork.GetRepository<BoatyardService>().GetPagingListAsync(
            selector: x => new GetBoatyardServicesByBoatyardIdResponse()
            {
                Id = x.Id,
                TypeService = x.TypeService,
                Price = x.Price,
                IsActive = x.IsActive,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate
            },
            predicate: x => x.BoatyardId == request.BoatyardId
                            && (string.IsNullOrEmpty(request.Name) || x.TypeService.Contains(request.Name)),
            page: request.Page,
            size: request.Size,
            sortBy: request.SortBy ?? nameof(BoatyardService.CreatedDate),
            isAsc: request.IsAsc
        );

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy danh sách dịch vụ xưởng thành công",
            Data = boatyardServices
        };
    }
}