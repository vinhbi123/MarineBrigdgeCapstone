using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Suppliers;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Suppliers.Query.GetSupplierDetail;

public class GetSupplierDetailQueryHandler : IRequestHandler<GetSupplierDetailQuery, ApiResponse>
{
    private readonly IClaimService _claimService;
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;

    public GetSupplierDetailQueryHandler(IClaimService claimService, IUnitOfWork<ShipCapstoneContext> unitOfWork)
    {
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async ValueTask<ApiResponse> Handle(GetSupplierDetailQuery request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }

        var supplier = await _unitOfWork.GetRepository<Supplier>().SingleOrDefaultAsync(
            predicate: x => x.AccountId == accountId,
            include: x => x.Include(x => x.Account)) ?? throw new NotFoundException("Không tìm thấy nhà cung cấp");

        var response = new GetSupplierDetailResponse()
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Latitude = supplier.Latitude,
            Longitude = supplier.Longitude,
            AccountId = supplier.AccountId,
            CommissionFeePercent = supplier.CommissionFeePercent,
            FullName = supplier.Account.FullName,
            Username = supplier.Account.Username,
            Address = supplier.Account.Address,
            Email = supplier.Account.Email,
            PhoneNumber = supplier.Account.PhoneNumber,
            AvatarUrl = supplier.Account.AvatarUrl,
            CreatedDate = supplier.CreatedDate,
            LastModifiedDate = supplier.LastModifiedDate
        };

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy chi tiết nhà cung cấp thành công",
            Data = response
        };
    }
}