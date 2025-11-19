using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Complaints.Command.CreateComplaint;

public class CreateComplaintCommand : IRequest<ApiResponse>
{
    public string Content { get; set; }
    public Guid? OrderId { get; set; }
    public Guid? BookingId { get; set; }
    public Guid ReceiverAccountId { get; set; } 
}
