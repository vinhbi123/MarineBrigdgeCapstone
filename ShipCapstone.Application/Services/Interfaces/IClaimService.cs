namespace ShipCapstone.Application.Services.Interfaces;

public interface IClaimService
{
    public Guid GetCurrentUserId { get; }
    public string GetCurrentUsername { get; }
    public string GetRole { get; }
    public string GetCurrentEmail { get; }
}