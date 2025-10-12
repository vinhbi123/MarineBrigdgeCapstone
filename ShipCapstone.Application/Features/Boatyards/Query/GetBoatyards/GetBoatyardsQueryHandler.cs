using Mediator;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Boatyards;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Boatyards.Query.GetBoatyards;

public class GetBoatyardsQueryHandler : IRequestHandler<GetBoatyardsQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    
    public GetBoatyardsQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async ValueTask<ApiResponse> Handle(GetBoatyardsQuery request, CancellationToken cancellationToken)
    {
        var boatyards = await _unitOfWork.GetRepository<Boatyard>().GetPagingListAsync(
            selector: x => new GetBoatyardsResponse()
            {
                Id = x.Id,
                Name = x.Name,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                AccountId = x.AccountId,
                FullName = x.Account.FullName,
                Username = x.Account.Username,
                Address = x.Account.Address,
                Email = x.Account.Email,
                PhoneNumber = x.Account.PhoneNumber,
                AvatarUrl = x.Account.AvatarUrl,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate
            },
            predicate: x => string.IsNullOrEmpty(request.Name) || x.Name.Contains(request.Name),
            page: request.Page,
            size: request.Size,
            sortBy: request.SortBy ?? nameof(Boatyard.CreatedDate),
            isAsc: request.IsAsc
        );

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy danh sách xưởng thành công",
            Data = boatyards
        };
    }
}