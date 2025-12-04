using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.ReportProblems;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ReportProblems.Query.GetReportProblemById;

public class GetReportProblemByIdQueryHandler : IRequestHandler<GetReportProblemByIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly IClaimService _claimService;
    public GetReportProblemByIdQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    public async ValueTask<ApiResponse> Handle(GetReportProblemByIdQuery request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        var role = Enum.Parse<ERole>(_claimService.GetRole);
        var reportProblem = await _unitOfWork.GetRepository<ReportProblem>().SingleOrDefaultAsync(
            predicate: rp => rp.Id == request.Id,
            include: rp => rp.Include(rp => rp.Ship)
                .ThenInclude(s => s.Account)
                .Include(rp => rp.Ship)
                .ThenInclude(s => s.Captain)
                .Include(rp => rp.Port)) ?? throw new NotFoundException("Không tìm thấy báo cáo sự cố");
        if (role == ERole.User && reportProblem.Ship.AccountId != accountId)
        {
            throw new BadHttpRequestException("Bạn không có quyền đọc báo cáo sự cố này");
        }
        if (role == ERole.Captain && reportProblem.Ship.CaptainId != accountId)
        {
            throw new BadHttpRequestException("Bạn không có quyền đọc báo cáo sự cố này");
        }

        var response = new GetAllReportProblemResponse()
        {
            Id = reportProblem.Id,
            ShipId = reportProblem.ShipId,
            ShipName = reportProblem.Ship.Name,
            CaptainId = reportProblem.Ship.CaptainId,
            CaptainName = reportProblem.Ship.Captain.FullName,
            PortId = reportProblem.PortId,
            PortName = reportProblem.Port.Name,
            Title = reportProblem.Title,
            Description = reportProblem.Description,
            Status = reportProblem.Status,
            CreatedDate = reportProblem.CreatedDate,
            LastModifiedDate = reportProblem.LastModifiedDate
        };
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy báo cáo sự cố thành công",
            Data = response
        };
    }
}