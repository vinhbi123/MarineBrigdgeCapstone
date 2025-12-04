using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ReportProblems.Command.CreateReportProblem;

public class CreateReportProblemCommandHandler : IRequestHandler<CreateReportProblemCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly IClaimService _claimService;

    public CreateReportProblemCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    public async ValueTask<ApiResponse> Handle(CreateReportProblemCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        var port = await _unitOfWork.GetRepository<Port>().SingleOrDefaultAsync(
            predicate: p => p.Id == request.PortId) ?? throw new NotFoundException("Không tìm thấy cảng");
        var ship = await _unitOfWork.GetRepository<Ship>().SingleOrDefaultAsync(
            predicate: s => s.CaptainId == accountId) ?? throw new NotFoundException("Không tìm thấy tàu");
        var reportProblem = new ReportProblem()
        {
            Id = Guid.CreateVersion7(),
            PortId = port.Id,
            ShipId = ship.Id,
            Title = request.Title,
            Description = request.Description,
            Status = EReportProblemStatus.Pending
        };
        await _unitOfWork.GetRepository<ReportProblem>().InsertAsync(reportProblem);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Có một lỗi trong quá trình tạo báo cáo vấn đề");
        }
        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Tạo báo cáo vấn đề thành công",
            Data = reportProblem.Id
        };
    }
}