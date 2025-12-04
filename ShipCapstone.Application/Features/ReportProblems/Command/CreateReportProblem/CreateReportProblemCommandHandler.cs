using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Domain.Models.ReportProblems;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.ReportProblems.Command.CreateReportProblem
{
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
            var currentAccountId = _claimService.GetCurrentUserId;
            if (currentAccountId == Guid.Empty)
                throw new BadHttpRequestException("Không tìm thấy tài khoản.");

            
            var ship = await _unitOfWork.GetRepository<Ship>()
                .SingleOrDefaultAsync<Ship>(
                    selector: s => s,
                    predicate: s => s.Id == request.ShipId,
                    orderBy: null,
                    include: null
                );

            if (ship == null)
                throw new NotFoundException("Không tìm thấy tàu.");

            if (!ship.CaptainId.HasValue || ship.CaptainId.Value != currentAccountId)
                throw new BadHttpRequestException("Bạn không phải là captain của tàu này.");

            var report = new ReportProblem
            {
                Id = Guid.NewGuid(),
                PortId = request.PortId,
                ShipId = request.ShipId,
                Title = request.Title?.Trim() ?? string.Empty,
                Description = request.Description?.Trim() ?? string.Empty,
                Status = EReportProblemStatus.Pending
            };

            await _unitOfWork.GetRepository<ReportProblem>().InsertAsync(report);

            var saved = await _unitOfWork.CommitAsync();
            if (saved <= 0)
                throw new Exception("Có lỗi khi tạo báo cáo sự cố.");

           
            var data = new ReportProblemResponse
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
                Status = StatusCodes.Status201Created,
                Message = "Tạo báo cáo sự cố thành công",
                Data = data
            };
        }
    }
}
