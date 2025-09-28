using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class ProductModifierGroupConfiguration : IEntityTypeConfiguration<ProductModifierGroup>
{
    public void Configure(EntityTypeBuilder<ProductModifierGroup> builder)
    {
        builder.HasKey(pmg => pmg.Id);
        builder.HasOne(pmg => pmg.Product)
            .WithMany(p => p.ProductModifierGroups)
            .HasForeignKey(pmg => pmg.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(pmg => pmg.ModifierGroup)
            .WithMany(mg => mg.ProductModifierGroups)
            .HasForeignKey(pmg => pmg.ModifierGroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}