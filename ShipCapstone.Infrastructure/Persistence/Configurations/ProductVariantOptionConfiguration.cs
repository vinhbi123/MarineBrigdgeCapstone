using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class ProductVariantOptionConfiguration : IEntityTypeConfiguration<ProductVariantOption>
{
    public void Configure(EntityTypeBuilder<ProductVariantOption> builder)
    {
        builder.HasKey(pvo => pvo.Id);
        builder.Property(pvo => pvo.ProductVariantId)
               .IsRequired();
        builder.Property(pvo => pvo.ModifierOptionId)
               .IsRequired();
        builder.HasOne(pvo => pvo.ProductVariant)
               .WithMany(pv => pv.ProductVariantOptions)
               .HasForeignKey(pvo => pvo.ProductVariantId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(pvo => pvo.ModifierOption)
               .WithMany(mo => mo.ProductVariantOptions)
               .HasForeignKey(pvo => pvo.ModifierOptionId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}