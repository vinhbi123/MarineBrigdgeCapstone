using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Domain.Models.Transactions;

public class GetTransactionsResponse
{
    public Guid Id { get; set; }
    public string? TransactionReference { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public ETransactionStatus Status { get; set; }
    public EPaymentType Type { get; set; }
}