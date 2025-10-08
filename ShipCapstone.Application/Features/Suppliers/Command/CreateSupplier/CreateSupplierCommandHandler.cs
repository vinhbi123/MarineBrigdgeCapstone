using Mediator;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Authentication;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Suppliers.Command.CreateSupplier;

public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IAuthenticationService _authenticationService;
    private readonly IUploadService _uploadService;
    private readonly IRedisService _redisService;

    public CreateSupplierCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger,
        IAuthenticationService authenticationService, IUploadService uploadService, IRedisService redisService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        _uploadService = uploadService ?? throw new ArgumentNullException(nameof(uploadService));
        _redisService = redisService ?? throw new ArgumentNullException(nameof(redisService));
    }
    
    public async ValueTask<ApiResponse> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var key = "otp:" + request.Email;
        var existingOtp = await _redisService.GetStringAsync(key);
        
        if (string.IsNullOrEmpty(existingOtp))
            throw new BadHttpRequestException("Không tìm thấy mã OTP");
        if (!existingOtp.Equals(request.Otp))
            throw new BadHttpRequestException("Mã OTP không chính xác");
        
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

        var supplier = new Supplier()
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Account = new Account()
            {
                Id = Guid.CreateVersion7(),
                FullName = request.FullName,
                Username = request.Username,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = PasswordUtil.HashPassword(request.Password),
                Role = ERole.Supplier,
                Address = request.Address
            }
        };
        if (request.Avatar != null)
        {
            var avatarUrl = await _uploadService.UploadImageAsync(request.Avatar);
            supplier.Account.AvatarUrl = avatarUrl;
        }
        
        await _unitOfWork.GetRepository<Supplier>().InsertAsync(supplier);

        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Tạo nhà cung cấp thất bại");
        }
        
        var token = _authenticationService.GenerateAccessToken(supplier.Account);
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Tạo nhà cung cấp thành công",
            Data = new LoginResponse()
            {
                AccountId = supplier.Account.Id,
                Email = supplier.Account.Email,
                Role = supplier.Account.Role,
                Username = supplier.Account.Username,
                AccessToken = token
            }
        };
    }
}