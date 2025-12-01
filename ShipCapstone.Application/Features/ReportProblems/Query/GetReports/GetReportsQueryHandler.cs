using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Features.ReportProblems.Query.GetReports;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.ReportProblems;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;
using System.Linq;

namespace ShipCapstone.Application.Features.ReportProblems.Query
{
    public class GetReportsQueryHandler : IRequestHandler<GetReportsQuery, ApiResponse>
    {
        private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
        private readonly IClaimService _claimService;

        public GetReportsQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, IClaimService claimService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        }

        public async ValueTask<ApiResponse> Handle(GetReportsQuery request, CancellationToken cancellationToken)
        {
            var accountId = _claimService.GetCurrentUserId;
            if (accountId == Guid.Empty)
                throw new BadHttpRequestException("Không tìm thấy tài khoản.");

            // get shipIds owned by current account (owner)
            var shipIdsPaging = await _unitOfWork.GetRepository<Ship>().GetPagingListAsync(
                selector: s => s.Id,
                predicate: s => s.AccountId == accountId,
                page: 1, size: int.MaxValue, sortBy: nameof(Ship.CreatedDate), isAsc: true
            );

            var shipIds = shipIdsPaging?.Items?.ToList() ?? new List<Guid>();
            if (!shipIds.Any())
                return new ApiResponse
                {
                    Status = StatusCodes.Status200OK,
                    Message = "No reports found",
                    Data = new { Items = new List<ReportProblemResponse>(), TotalItems = 0, Page = request.Page, PageSize = request.PageSize }
                };

            var reports = await _unitOfWork.GetRepository<ReportProblem>().GetPagingListAsync(
                selector: r => new ReportProblemResponse
                {
                    Id = r.Id,
                    PortId = r.PortId,
                    ShipId = r.ShipId,
                    Title = r.Title,
                    Description = r.Description,
                    Status = r.Status,
                    CreatedDate = r.CreatedDate
                },
                predicate: r =>
                    shipIds.Contains(r.ShipId) &&
                    (!request.ShipId.HasValue || r.ShipId == request.ShipId) &&
                    (string.IsNullOrEmpty(request.Status) || r.Status.ToString() == request.Status) &&
                    (string.IsNullOrEmpty(request.Search) || r.Title.Contains(request.Search) || r.Description.Contains(request.Search)),
                page: request.Page,
                size: request.PageSize,
                sortBy: request.SortBy ?? nameof(ReportProblem.CreatedDate),
                isAsc: request.IsAsc
            ) ?? throw new NotFoundException("Không tìm thấy báo cáo.");

            return new ApiResponse
            {
                Status = StatusCodes.Status200OK,
                Message = "Lấy danh sách báo cáo thành công",
                Data = reports
            };
        }
    }
}
