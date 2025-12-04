using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ReportProblems.Command.DeleteReportProblem;

public class DeleteReportProblemCommandHandler : IRequestHandler<DeleteReportProblemCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly IClaimService _claimService;

    public DeleteReportProblemCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }

    public async ValueTask<ApiResponse> Handle(DeleteReportProblemCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        var reportProblem = await _unitOfWork.GetRepository<ReportProblem>().SingleOrDefaultAsync(
            predicate: rp => rp.Id == request.Id,
            include: rp => rp.Include(rp => rp.Ship)) ?? throw new NotFoundException("Không tìm thấy báo cáo sự cố");
        if (reportProblem.Ship.CaptainId != accountId)
        {
            throw new BadHttpRequestException("Bạn không quyền xóa báo cáo sự cố này");
        }

        if (reportProblem.Status != EReportProblemStatus.Pending)
        {
            throw new BadHttpRequestException("Không được xóa báo cáo sự cố với trạng thái là đang chờ");
        }
        _unitOfWork.GetRepository<ReportProblem>().DeleteAsync(reportProblem);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Có lỗi xảy ra trong quá trình xóa báo cáo sự cố");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Xóa báo cáo sự cố thành công",
            Data = null
        };
    }
}