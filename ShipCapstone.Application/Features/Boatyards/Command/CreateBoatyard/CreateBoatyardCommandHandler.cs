using Mediator;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Boatyards.Command.CreateBoatyard;

public class CreateBoatyardCommandHandler : IRequestHandler<CreateBoatyardCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IRedisService _redisService;
    private readonly IUploadService _uploadService;

    public CreateBoatyardCommandHandler(
        IUnitOfWork<ShipCapstoneContext> unitOfWork,
        ILogger logger,
        IRedisService redisService,
        IUploadService uploadService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _redisService = redisService ?? throw new ArgumentNullException(nameof(redisService));
        _uploadService = uploadService ?? throw new ArgumentNullException(nameof(uploadService));
    }

    public async ValueTask<ApiResponse> Handle(CreateBoatyardCommand request, CancellationToken cancellationToken)
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

        var boatyard = new Boatyard()
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
                Address = request.Address,
                PasswordHash = PasswordUtil.HashPassword(request.Password),
                Role = ERole.Boatyard
            },
            DockSlots = request.DockSlots.Select(x => new DockSlot()
            {
                Id = Guid.CreateVersion7(),
                Name = x.Name,
                AssignedFrom = x.AssignedFrom,
                AssignedUntil = x.AssignedUntil,
                IsActive = true
            }).ToList()
        };

        if (request.Avatar != null)
        {
            var avatarUrl = await _uploadService.UploadImageAsync(request.Avatar);
            boatyard.Account.AvatarUrl = avatarUrl;
        }
        await _unitOfWork.GetRepository<Boatyard>().InsertAsync(boatyard);

        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            _logger.Error("CreateBoatyardCommandHandler - Handle: Tạo bến thuyền thất bại");
            throw new Exception("Tạo bến thuyền thất bại");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Tạo bến thuyền thành công",
            Data = boatyard.Id
        };
    }
}