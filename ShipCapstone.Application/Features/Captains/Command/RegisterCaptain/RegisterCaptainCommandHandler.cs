using Mediator;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Authentication;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Captains.Command.RegisterCaptain;

public class RegisterCaptainCommandHandler : IRequestHandler<RegisterCaptainCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly IUploadService _uploadService;
    private readonly IAuthenticationService _authenticationService;

    public RegisterCaptainCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, 
        IUploadService uploadService, IAuthenticationService authenticationService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _uploadService = uploadService ?? throw new ArgumentNullException(nameof(uploadService));
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
    }
    
    public async ValueTask<ApiResponse> Handle(RegisterCaptainCommand request, CancellationToken cancellationToken)
    {
        var existingAccount = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: x => x.Email.Equals(request.Email) || x.Username.Equals(request.Username) || 
                            (request.PhoneNumber != null && x.PhoneNumber != null && x.PhoneNumber.Equals(request.PhoneNumber))
        );
        if (existingAccount != null)
        {
            if (existingAccount.Email.Equals(request.Email))
                throw new BadHttpRequestException("Email đã được sử dụng");
            if (existingAccount.Username.Equals(request.Username))
                throw new BadHttpRequestException("Tên đăng nhập đã được sử dụng");
            if (request.PhoneNumber != null && existingAccount.PhoneNumber != null && existingAccount.PhoneNumber.Equals(request.PhoneNumber))
                throw new BadHttpRequestException("Số điện thoại đã được sử dụng");
        }
        
        var account = new Account
        {
            Id = Guid.CreateVersion7(),
            FullName = request.FullName,
            Username = request.Username,
            Email = request.Email,
            PasswordHash = PasswordUtil.HashPassword(request.Password),
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Role = ERole.Captain
        };

        if (request.Avatar != null)
        {
            var avatarUrl = await _uploadService.UploadImageAsync(request.Avatar);
            account.AvatarUrl = avatarUrl;
        }

        await _unitOfWork.GetRepository<Account>().InsertAsync(account);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
            throw new Exception("Một lỗi đã xảy ra trong quá trình đăng ký tài khoản thuyền trưởng");

        var accessToken = _authenticationService.GenerateAccessToken(account);
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Đăng ký tài khoản thành công",
            Data = new LoginResponse()
            {
                Email = account.Email,
                Username = account.Username,
                AccountId = account.Id,
                Role = account.Role,
                AccessToken = accessToken
            }
        };
    }
}