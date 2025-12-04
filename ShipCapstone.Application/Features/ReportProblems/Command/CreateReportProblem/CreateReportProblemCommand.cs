using Mediator;
using ShipCapstone.Domain.Models.Common;
using System;

namespace ShipCapstone.Application.Features.ReportProblems.Command.CreateReportProblem
{
    public class CreateReportProblemCommand : IRequest<ApiResponse>
    {
        public Guid PortId { get; set; }
        public Guid ShipId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
