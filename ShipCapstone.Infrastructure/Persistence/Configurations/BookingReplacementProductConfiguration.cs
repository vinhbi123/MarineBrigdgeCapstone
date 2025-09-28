using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class BookingReplacementProductConfiguration : IEntityTypeConfiguration<BookingReplacementProduct>
{
    public void Configure(EntityTypeBuilder<BookingReplacementProduct> builder)
    {
        builder.HasKey(brp => brp.Id);
        builder.Property(brp => brp.Quantity).IsRequired();
        builder.Property(brp => brp.Note).HasMaxLength(500);
        builder.HasOne(brp => brp.BookingService)
            .WithMany(bs => bs.BookingReplacementProducts)
            .HasForeignKey(brp => brp.BookingServiceId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(brp => brp.ProductVariant)
            .WithMany(pv => pv.BookingReplacementProducts)
            .HasForeignKey(brp => brp.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}