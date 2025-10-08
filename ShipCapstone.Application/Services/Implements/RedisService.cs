using ShipCapstone.Application.Services.Interfaces;
using StackExchange.Redis;

namespace ShipCapstone.Application.Services.Implements;

public class RedisService : IRedisService
{
    private readonly IDatabase _db;
    public RedisService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }
    
    public async Task<string?> GetStringAsync(string key)
    {
        return await _db.StringGetAsync(key);
    }
    
    public async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null)
    {
        return await _db.StringSetAsync(key, value, expiry);
    }
    
}