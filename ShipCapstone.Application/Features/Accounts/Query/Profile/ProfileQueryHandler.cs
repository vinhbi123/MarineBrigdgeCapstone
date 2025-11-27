using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Profile;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Accounts.Query.Profile
{
    public class ProfileQueryHandler : IRequestHandler<ProfileQuery, ApiResponse>
    {
        private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
        private readonly IClaimService _claimService;
        public ProfileQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, IClaimService claimService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        }

        public async ValueTask<ApiResponse> Handle(ProfileQuery request, CancellationToken cancellationToken)
        {
            var accountId = _claimService.GetCurrentUserId;
            if (accountId == Guid.Empty)
            {
                throw new BadHttpRequestException("Không tìm thấy tài khoản.");
            }
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                selector: a => new GetProfileResponse
                {
                    Id = a.Id,
                    Address = a.Address,
                    AvatarUrl = a.AvatarUrl,
                    FullName = a.FullName,
                    PhoneNumber = a.PhoneNumber
                },
                predicate: a => a.Id == accountId) ?? throw new NotFoundException("Không tìm thấy tài khoản.");
            return new ApiResponse
            {
                Status = StatusCodes.Status200OK,
                Message = "Lấy thông tin hồ sơ thành công",
                Data = account,
            };
        }
    }
}