using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Reports.Command.CreateReportProblem;

public class CreateReportProblemCommandHandler : IRequestHandler<CreateReportProblemCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;

    public CreateReportProblemCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }

    public async ValueTask<ApiResponse> Handle(CreateReportProblemCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        if (accountId == Guid.Empty)
        {
            throw new BadHttpRequestException("Tài khoản không hợp lệ");
        }

        // Validate port exists
        var port = await _unitOfWork.GetRepository<Port>()
            .SingleOrDefaultAsync(predicate: p => p.Id == request.PortId);
        if (port == null)
            throw new NotFoundException("Không tìm thấy cảng (port)");

        // Validate ship exists
        var ship = await _unitOfWork.GetRepository<Ship>()
            .SingleOrDefaultAsync(predicate: s => s.Id == request.ShipId);
        if (ship == null)
            throw new NotFoundException("Không tìm thấy tàu (ship)");

        var report = new ReportProblem()
        {
            Id = Guid.CreateVersion7(),
            PortId = request.PortId,
            ShipId = request.ShipId,
            Title = request.Title,
            Description = request.Description,
            Status = EReportProblemStatus.Pending, // nếu enum khác, thay cho phù hợp
            // EntityAuditBase có thể tự set CreatedBy/CreatedDate nếu bạn có logic EF core;
            // nếu không, bạn có thể set CreatedBy = accountId nếu trường tồn tại.
        };

        await _unitOfWork.GetRepository<ReportProblem>().InsertAsync(report);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Gửi báo cáo thất bại");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Gửi báo cáo thành công",
            Data = report.Id
        };
    }
}
