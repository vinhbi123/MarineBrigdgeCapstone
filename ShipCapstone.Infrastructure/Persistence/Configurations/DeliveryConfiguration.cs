using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.DepartureTime).IsRequired();
        builder.Property(d => d.Status).IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (EDeliveryStatus)Enum.Parse(typeof(EDeliveryStatus), v));
        builder.HasOne(d => d.Ship)
            .WithMany(s => s.Deliveries)
            .HasForeignKey(d => d.ShipId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(d => d.Order)
            .WithMany(o => o.Deliveries)
            .HasForeignKey(d => d.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}