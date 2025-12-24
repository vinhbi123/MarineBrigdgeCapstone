namespace ShipCapstone.Domain.Models.Revenues;

public class GetRevenueResponse
{
    public string Month { get; set; }
    public string Year { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal NetRevenue { get; set; }
    public bool IsTransferred { get; set; }
    public DateTime? TransferredDate { get; set; }
}