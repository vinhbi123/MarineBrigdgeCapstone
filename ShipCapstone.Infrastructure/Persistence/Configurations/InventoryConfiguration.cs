using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Quantity).IsRequired();
        builder.HasOne(i => i.ProductVariant)
            .WithMany(pv => pv.Inventories)
            .HasForeignKey(i => i.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(i => i.ModifierOption)
            .WithMany(mo => mo.Inventories)
            .HasForeignKey(i => i.ModifierOptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}