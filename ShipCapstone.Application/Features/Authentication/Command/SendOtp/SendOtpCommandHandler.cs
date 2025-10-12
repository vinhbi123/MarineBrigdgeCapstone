using Mediator;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.Emails;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Authentication.Command.SendOtp;

public class SendOtpCommandHandler : IRequestHandler<SendOtpCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IRedisService _redisService;
    private readonly IEmailService _emailService;
    private readonly IWebHostEnvironment _env;

    public SendOtpCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork,
        ILogger logger, IRedisService redisService, IEmailService emailService,
        IWebHostEnvironment env)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _redisService = redisService ?? throw new ArgumentNullException(nameof(redisService));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _env = env ?? throw new ArgumentNullException(nameof(env));
    }
    
    public async ValueTask<ApiResponse> Handle(SendOtpCommand request, CancellationToken cancellationToken)
    {
        var key = "otp:" + request.Email;
        var existingOtp = await _redisService.GetStringAsync(key);
        
        if (!string.IsNullOrEmpty(existingOtp))
            throw new BadHttpRequestException("Mã OTP đã được gửi");
        
        var existingAccount = await _unitOfWork.GetRepository<Domain.Entities.Account>().SingleOrDefaultAsync(
            predicate: x => x.Email.Equals(request.Email)
        );
        if (existingAccount != null)
        {
            throw new BadHttpRequestException("Email đã được sử dụng");
        }
        
        var otp = OtpUtil.GenerateOtp();
        
        var emailTemplate = GetTemplate(request.Email, otp);
        var emailMessage = new EmailMessage()
        {
            ToAddress = request.Email,
            Body = emailTemplate,
            Subject = otp + " là mã xác thực của bạn",
        };
        await _emailService.SendEmailAsync(emailMessage);

        var isSuccess = await _redisService.SetStringAsync(key, otp, TimeSpan.FromMinutes(5));

        if (!isSuccess)
            throw new BadHttpRequestException("Lỗi khi lưu mã OTP");
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Gửi mã OTP thành công",
            Data = request.Email
        };
    }
    private string GetTemplate(string email, string otp)
    {
        var templatePath = Path.Combine(_env.WebRootPath, "assets/html", "template.html");
        if (!File.Exists(templatePath))
            throw new FileNotFoundException("Email template not found", templatePath);
        
        var template = File.ReadAllText(templatePath);
        template = template.Replace("{Email}", email);
        template = template.Replace("{otp}", otp);
        return template;
    }
}