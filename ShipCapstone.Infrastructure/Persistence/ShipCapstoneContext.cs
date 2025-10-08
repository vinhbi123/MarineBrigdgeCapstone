using Microsoft.EntityFrameworkCore;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Entities.Common.Interface;
using ShipCapstone.Infrastructure.Utils;

namespace ShipCapstone.Infrastructure.Persistence;

public class ShipCapstoneContext : DbContext
{
    public ShipCapstoneContext() {}
    
    public ShipCapstoneContext(DbContextOptions<ShipCapstoneContext> options) : base(options) { }
    
    public virtual DbSet<Account> Account { get; set; }
    public virtual DbSet<Boatyard> Boatyard { get; set; }
    public virtual DbSet<BoatyardReview> BoatyardReview { get; set; }
    public virtual DbSet<BoatyardService> BoatyardService { get; set; }
    public virtual DbSet<Booking> Booking { get; set; }
    public virtual DbSet<BookingReplacementProduct> BookingReplacementProduct { get; set; }
    public virtual DbSet<BookingService> BookingService { get; set; }
    public virtual DbSet<Category> Category { get; set; }
    public virtual DbSet<Complaint> Complaint { get; set; }
    public virtual DbSet<Delivery> Delivery { get; set; }
    public virtual DbSet<DockSlot> DockSlot { get; set; }
    public virtual DbSet<Inventory> Inventory { get; set; }
    public virtual DbSet<ModifierGroup> ModifierGroup { get; set; }
    public virtual DbSet<ModifierOption> ModifierOption { get; set; }
    public virtual DbSet<Notification> Notification { get; set; }
    public virtual DbSet<Order> Order { get; set; }
    public virtual DbSet<OrderItem> OrderItem { get; set; }
    public virtual DbSet<Port> Port { get; set; }
    public virtual DbSet<Product> Product { get; set; }
    public virtual DbSet<ProductImage> ProductImage { get; set; }
    public virtual DbSet<ProductModifierGroup> ProductModifierGroup { get; set; }
    public virtual DbSet<ProductVariant> ProductVariant { get; set; }
    public virtual DbSet<Review> Review { get; set; }
    public virtual DbSet<Ship> Ship { get; set; }
    public virtual DbSet<ShipPortHistory> ShipPortHistory { get; set; }
    public virtual DbSet<Supplier> Supplier { get; set; }
    public virtual DbSet<Transaction> Transaction { get; set; }
    public virtual DbSet<Payment> Payment { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShipCapstoneContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
        try
        { 
            var modified = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified ||
                            e.State == EntityState.Added ||
                            e.State == EntityState.Deleted);

            foreach (var item in modified)
                switch (item.State)
                {
                    case EntityState.Added:
                        if (item.Entity is IDateTracking addedEntity)
                        {
                            addedEntity.CreatedDate = TimeUtil.GetCurrentSEATime();
                            item.State = EntityState.Added;
                        }

                        break;
                    case EntityState.Modified:
                        if (item.Entity is IDateTracking modifiedEntity)
                        {
                            Entry(item.Entity).Property("Id").IsModified = false;
                            modifiedEntity.LastModifiedDate = TimeUtil.GetCurrentSEATime();
                            item.State = EntityState.Modified;
                        }

                        break;
                }

            var result = await base.SaveChangesAsync(cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
            
            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}