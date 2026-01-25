using Mediator;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Accounts.Command.UpdateComissionFee;

public class UpdateComissionFeeCommand : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
    public ETypeUpdate Type { get; set; }
    public decimal? CommissionFeePercent { get; set; }
}

public class UpdateCommissionFeeRequest
{
    public ETypeUpdate Type { get; set; }
    public decimal? CommissionFeePercent { get; set; }
}