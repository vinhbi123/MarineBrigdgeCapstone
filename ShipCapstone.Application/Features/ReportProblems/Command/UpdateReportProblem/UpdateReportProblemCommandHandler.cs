using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ReportProblems.Command.UpdateReportProblem;

public class UpdateReportProblemCommandHandler : IRequestHandler<UpdateReportProblemCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly IClaimService _claimService;

    public UpdateReportProblemCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }

    public async ValueTask<ApiResponse> Handle(UpdateReportProblemCommand request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        var reportProblem = await _unitOfWork.GetRepository<ReportProblem>().SingleOrDefaultAsync(
            predicate: rp => rp.Id == request.Id,
            include: rp => rp.Include(rp => rp.Ship)) ?? throw new NotFoundException("Không tìm thấy báo cáo sự cố");
        if (reportProblem.Ship.AccountId != accountId)
        {
            throw new BadHttpRequestException("Bạn không quyền chỉnh sửa báo cáo sự cố này");
        }
        reportProblem.Status = request.Status ?? reportProblem.Status;
        _unitOfWork.GetRepository<ReportProblem>().UpdateAsync(reportProblem);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Có lỗi xảy ra trong quá trình chỉnh sửa báo cáo sự cố");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Cập nhật trạng thái thành công",
            Data = reportProblem.Id
        };
    }
}