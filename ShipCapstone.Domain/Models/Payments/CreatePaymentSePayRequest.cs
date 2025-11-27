namespace ShipCapstone.Domain.Models.Payments;

public class CreatePaymentSePayRequest
{
    public string BankName { get; set; }
    public string BankNo { get; set; }
    public decimal Revenue { get; set; }
    public string Description { get; set; }
}