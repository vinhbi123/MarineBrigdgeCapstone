using Net.payOS.Types;
using ShipCapstone.Domain.Models.Payments;

namespace ShipCapstone.Application.Services.Interfaces;

public interface IPaymentService
{
    Task<CreatePaymentResult> CreatePaymentUrl(CreatePaymentRequest request);
}