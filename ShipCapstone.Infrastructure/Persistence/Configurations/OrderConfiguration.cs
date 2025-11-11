using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;

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
        builder.Property(rp => rp.Status)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (EOrderStatus)Enum.Parse(typeof(EOrderStatus), v)
            );
    }
}