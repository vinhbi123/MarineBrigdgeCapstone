namespace ShipCapstone.Application.Services.Interfaces;

public interface IRedisService
{
    Task<string?> GetStringAsync(string key);
    
    Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null);
}