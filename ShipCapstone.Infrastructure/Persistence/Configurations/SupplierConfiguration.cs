using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(255);
        builder.Property(s => s.Longitude).HasMaxLength(50);
        builder.Property(s => s.Latitude).HasMaxLength(50);
        builder.HasOne(s => s.Account)
            .WithOne(a => a.Supplier)
            .HasForeignKey<Supplier>(s => s.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}