using Mediator;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Suppliers;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Suppliers.Query.GetSuppliers;

public class GetSuppliersQueryHandler : IRequestHandler<GetSuppliersQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    
    public GetSuppliersQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async ValueTask<ApiResponse> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        var suppliers = await _unitOfWork.GetRepository<Supplier>().GetPagingListAsync(
            selector: x => new GetSuppliersResponse()
            {
                Id = x.Id,
                Name = x.Name,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                AccountId = x.AccountId,
                FullName = x.Account.FullName,
                Username = x.Account.Username,
                PhoneNumber = x.Account.PhoneNumber,
                Email = x.Account.Email,
                Address = x.Account.Address,
                AvatarUrl = x.Account.AvatarUrl,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate
            },
            predicate: x => string.IsNullOrEmpty(request.Name) || x.Name.Contains(request.Name),
            page: request.Page,
            size: request.Size,
            sortBy: request.SortBy ?? nameof(Supplier.CreatedDate),
            isAsc: request.IsAsc
        );
        
        return new ApiResponse
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy danh sách nhà cung cấp thành công",
            Data = suppliers
        };
        
    }
}