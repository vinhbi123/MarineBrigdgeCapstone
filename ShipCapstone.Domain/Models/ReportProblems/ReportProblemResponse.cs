using ShipCapstone.Domain.Enums;
using System;

namespace ShipCapstone.Domain.Models.ReportProblems
{
    public class ReportProblemResponse
    {
        public Guid Id { get; set; }
        public Guid PortId { get; set; }
        public Guid ShipId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public EReportProblemStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
