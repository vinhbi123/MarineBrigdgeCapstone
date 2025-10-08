using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Authentication;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Authentication.Command.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IAuthenticationService _authenticationService;
    
    public LoginCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork,
        ILogger logger, IAuthenticationService authenticationService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _authenticationService =
            authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
    }
    
    public async ValueTask<ApiResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: x => x.Email.Equals(request.UsernameOrEmail) || x.Username.Equals(request.UsernameOrEmail)
        );

        if (account == null)
        {
            throw new NotFoundException("Không tìm thấy thông tin tài khoản");
        }

        if (!account.PasswordHash.Equals(PasswordUtil.HashPassword(request.Password)))
        {
            throw new BadHttpRequestException("Tên đăng nhập hoặc mật khẩu không đúng");
        }

        var token = _authenticationService.GenerateAccessToken(account);
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Đăng nhập thành công",
            Data = new LoginResponse()
            {
                AccountId = account.Id,
                Username = account.Username,
                Email = account.Email,
                AccessToken = token,
                Role = account.Role
            }
        };
    }
}