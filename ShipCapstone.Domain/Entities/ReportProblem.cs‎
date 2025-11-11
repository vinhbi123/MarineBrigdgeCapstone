using ShipCapstone.Domain.Entities.Common;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Domain.Entities;

public class ReportProblem : EntityAuditBase<Guid>
{
    public Guid PortId { get; set; }
    public Guid ShipId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public EReportProblemStatus Status { get; set; }
    public virtual Port Port { get; set; }
    public virtual Ship Ship { get; set; }
}