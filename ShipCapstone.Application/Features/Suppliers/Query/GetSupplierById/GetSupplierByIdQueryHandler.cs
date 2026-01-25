using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Suppliers;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Suppliers.Query.GetSupplierById;

public class GetSupplierByIdQueryHandler : IRequestHandler<GetSupplierByIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    
    public GetSupplierByIdQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    public async ValueTask<ApiResponse> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
    {
        var supplier = await _unitOfWork.GetRepository<Supplier>().SingleOrDefaultAsync(
            selector: x => new GetSuppliersResponse()
            {
                Id = x.Id,
                Name = x.Name,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                AccountId = x.AccountId,
                CommissionFeePercent = x.CommissionFeePercent,
                FullName = x.Account.FullName,
                Username = x.Account.Username,
                PhoneNumber = x.Account.PhoneNumber,
                Email = x.Account.Email,
                Address = x.Account.Address,
                AvatarUrl = x.Account.AvatarUrl,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate
            },
            predicate: x => x.Id == request.Id
        ) ?? throw new NotFoundException("Không tìm thấy nhà cung cấp");

        return new ApiResponse
        {
            Status = StatusCodes.Status200OK,
            Message = "Tìm thấy nhà cung cấp",
            Data = supplier
        };
    }
}