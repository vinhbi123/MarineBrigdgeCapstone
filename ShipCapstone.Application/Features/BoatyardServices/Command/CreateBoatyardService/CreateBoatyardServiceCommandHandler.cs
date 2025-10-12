using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.BoatyardServices.Command.CreateBoatyardService;

public class CreateBoatyardServiceCommandHandler : IRequestHandler<CreateBoatyardServiceCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public CreateBoatyardServiceCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger,
        IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(CreateBoatyardServiceCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }
        
        var boatyard = await _unitOfWork.GetRepository<Boatyard>().SingleOrDefaultAsync(
            predicate: x => x.AccountId == accountId
        );
        if (boatyard == null)
        {
            throw new NotFoundException("Không tìm thấy xưởng");
        }
        
        var boatyardService = new BoatyardService()
        {
            Id = Guid.CreateVersion7(),
            TypeService = request.TypeService,
            Price = request.Price,
            IsActive = true,
            BoatyardId = boatyard.Id
        };
        
        await _unitOfWork.GetRepository<BoatyardService>().InsertAsync(boatyardService);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Tạo dịch vụ xưởng thất bại");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Tạo dịch vụ xưởng thành công",
            Data = boatyardService.Id
        };
    }
}