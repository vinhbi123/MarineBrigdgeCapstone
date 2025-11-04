namespace ShipCapstone.Domain.Models.Reviews;

public class GetReviewResponse
{
    public Guid Id { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public Guid AccountId { get; set; }
    public Guid ProductId { get; set; }
    public string? AccountFullName { get; set; }   
    public string? AccountAvatarUrl { get; set; } 
    public DateTime CreatedDate { get; set; }
}
