using Mediator;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Authentication;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Authentication.Command.Oauth;

public class SignInGoogleCommandHandler : IRequestHandler<SignInGoogleCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IOAuthService _oAuthService;
    private readonly IAuthenticationService _authenticationService;

    public SignInGoogleCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork,
        ILogger logger,
        IOAuthService oAuthService, IAuthenticationService authenticationService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _oAuthService = oAuthService;
        _authenticationService = authenticationService;
    }
    
    public async ValueTask<ApiResponse> Handle(SignInGoogleCommand request, CancellationToken cancellationToken)
    {
        var googleUser = await _oAuthService.GetUserByCode(request.Code, request.IsAndroid);

        if (googleUser == null)
        {
            throw new Exception("Xác thực google thất bại");
        }
        
        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: a => a.Email.Equals(googleUser.Email));
        if (account == null)
        {
            account = new Account()
            {
                Id = Guid.CreateVersion7(),
                Username = googleUser.Email.Split('@')[0],
                PasswordHash = PasswordUtil.HashPassword((DateTime.Now.Ticks % 1000000000000000L).ToString()),
                Email = googleUser.Email,
                FullName = googleUser.Name,
                AvatarUrl = googleUser.Picture,
                Role = ERole.User
            };
            await _unitOfWork.GetRepository<Account>().InsertAsync(account);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess)
            {
                throw new Exception("Có lỗi xảy ra khi tạo tài khoản mới");
            }
        }

        var token = _authenticationService.GenerateAccessToken(account);
        return new ApiResponse
        {
            Status = StatusCodes.Status200OK,
            Message = "Đăng nhập google thành công",
            Data = new LoginResponse()
            {
                AccountId = account.Id,
                Email = account.Email,
                Username = account.Username,
                Role = account.Role,
                AccessToken = token
            }
        };
    }
}