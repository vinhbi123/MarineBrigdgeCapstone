using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.TotalAmount).IsRequired().HasPrecision(12, 2);
        builder.HasOne(o => o.Ship)
            .WithMany(s => s.Orders)
            .HasForeignKey(o => o.ShipId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}