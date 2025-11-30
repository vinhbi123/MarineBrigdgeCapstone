namespace ShipCapstone.Domain.Models.Review;

public class GetReviewResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string? ProductName { get; set; }
    public Guid AccountId { get; set; }
    public string? AccountName { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedDate { get; set; }
}