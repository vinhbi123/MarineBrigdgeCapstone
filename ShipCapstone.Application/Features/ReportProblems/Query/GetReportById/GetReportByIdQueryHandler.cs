using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Features.ReportProblems.Query.GetReportById;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.ReportProblems;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ReportProblems.Query
{
    public class GetReportByIdQueryHandler : IRequestHandler<GetReportByIdQuery, ApiResponse>
    {
        private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
        private readonly IClaimService _claimService;

        public GetReportByIdQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, IClaimService claimService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        }

        public async ValueTask<ApiResponse> Handle(GetReportByIdQuery request, CancellationToken cancellationToken)
        {
            var accountId = _claimService.GetCurrentUserId;
            if (accountId == Guid.Empty)
                throw new BadHttpRequestException("Không tìm thấy tài khoản.");

            // load report with Ship included to check owner
            var report = await _unitOfWork.GetRepository<ReportProblem>()
                .SingleOrDefaultAsync<ReportProblem>(
                    selector: r => r,
                    predicate: r => r.Id == request.Id,
                    orderBy: null,
                    include: q => q.Include(r => r.Ship)
                );

            if (report == null)
                throw new NotFoundException("Không tìm thấy báo cáo.");

            if (report.Ship == null || report.Ship.AccountId != accountId)
                throw new BadHttpRequestException("Bạn không có quyền xem báo cáo này.");

            var data = new GetAllReportProblemResponse
            {
                Id = report.Id,
                PortId = report.PortId,
                ShipId = report.ShipId,
                Title = report.Title,
                Description = report.Description,
                Status = report.Status,
                CreatedDate = report.CreatedDate
            };

            return new ApiResponse
            {
                Status = StatusCodes.Status200OK,
                Message = "Lấy thông tin báo cáo thành công",
                Data = data
            };
        }
    }
}
