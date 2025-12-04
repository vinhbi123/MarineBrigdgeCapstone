using Mediator;
using ShipCapstone.Domain.Models.Common;
using System;

namespace ShipCapstone.Application.Features.ReportProblems.Query.GetReportById
{
    public class GetReportByIdQuery : IRequest<ApiResponse>
    {
        public Guid Id { get; set; }
    }
}
