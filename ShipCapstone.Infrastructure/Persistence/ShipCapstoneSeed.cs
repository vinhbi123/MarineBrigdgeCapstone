using Microsoft.EntityFrameworkCore;

namespace ShipCapstone.Infrastructure.Persistence;

public class ShipCapstoneSeed
{
    private readonly ILogger _logger;
    private readonly ShipCapstoneContext _context;
    
    public ShipCapstoneSeed(ILogger logger, ShipCapstoneContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public async Task InitializeAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception e)
        {
            _logger.Error(e, "An error occurred while migrating the database");
            throw;
        }
    }
}