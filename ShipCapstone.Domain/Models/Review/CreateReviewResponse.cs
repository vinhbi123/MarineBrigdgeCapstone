namespace ShipCapstone.Domain.Models.Review;

public class CreateReviewResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid AccountId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}