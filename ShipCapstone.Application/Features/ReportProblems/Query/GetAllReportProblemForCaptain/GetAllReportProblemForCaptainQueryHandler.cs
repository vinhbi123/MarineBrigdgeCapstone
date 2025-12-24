using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.ReportProblems;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ReportProblems.Query.GetAllReportProblemForCaptain;

public class GetAllReportProblemForCaptainQueryHandler : IRequestHandler<GetAllReportProblemForCaptainQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly IClaimService _claimService;
    public GetAllReportProblemForCaptainQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(GetAllReportProblemForCaptainQuery request, CancellationToken cancellationToken)
    {
        var accountId = _claimService.GetCurrentUserId;
        var ship = await _unitOfWork.GetRepository<Ship>().SingleOrDefaultAsync(
            predicate: s => s.CaptainId == accountId,
            include: s => s.Include(s => s.Captain)) ?? throw new NotFoundException("Không tìm thấy tàu");
        var reportProblems = await _unitOfWork.GetRepository<ReportProblem>().GetPagingListAsync(
            selector: rp => new GetAllReportProblemResponse()
            {
                Id = rp.Id,
                ShipId = rp.ShipId,
                ShipName = ship.Name,
                CaptainId = ship.CaptainId,
                CaptainName = ship.Captain.FullName,
                PortId = rp.PortId,
                PortName = rp.Port.Name,
                Title = rp.Title,
                Description = rp.Description,
                Status = rp.Status,
                CreatedDate = rp.CreatedDate,
                LastModifiedDate = rp.LastModifiedDate
            },
            predicate: rp => rp.ShipId == ship.Id,
            page: request.Page,
            size: request.Size,
            sortBy: request.SortBy ?? nameof(Category.CreatedDate),
            isAsc: request.IsAsc);
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy danh sách báo cáo sự cố thành công",
            Data = reportProblems
        };
    }
}