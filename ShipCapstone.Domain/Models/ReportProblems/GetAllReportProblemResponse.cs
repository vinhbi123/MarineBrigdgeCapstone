using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Domain.Models.ReportProblems;

public class GetAllReportProblemResponse
{
    public Guid Id { get; set; }
    public Guid ShipId { get; set; }
    public string? ShipName { get; set; }
    public Guid? CaptainId { get; set; }
    public string? CaptainName { get; set; }
    public Guid PortId { get; set; }
    public string? PortName { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public EReportProblemStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}