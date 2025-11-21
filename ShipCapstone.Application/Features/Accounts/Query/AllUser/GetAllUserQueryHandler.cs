using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Profile;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Accounts.Query.AllUser;

    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, ApiResponse>
    {
        private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
        private readonly IClaimService _claimService;
        public GetAllUserQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, IClaimService claimService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        }

        public async ValueTask<ApiResponse> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.GetRepository<Account>().GetPagingListAsync(
                selector: a => new GetProfileUserResponse
                {
                    Id = a.Id,
                    Address = a.Address,
                    AvatarUrl = a.AvatarUrl,
                    FullName = a.FullName,
                    PhoneNumber = a.PhoneNumber
                },
                predicate: a => string.IsNullOrEmpty(request.Name) || a.FullName.Contains(request.Name),
                page: request.Page,
                size: request.Size,
                sortBy: request.SortBy ?? nameof(Account.CreatedDate),
                isAsc: request.IsAsc
            ) ?? throw new NotFoundException("Không tìm thấy tài khoản.");
        return new ApiResponse
            {
                Status = StatusCodes.Status200OK,
                Message = "Lấy danh sách người dùng thành công",
                Data = account,
            };
        }
    }


