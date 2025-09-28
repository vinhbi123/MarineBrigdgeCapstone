using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class ShipConfiguration : IEntityTypeConfiguration<Ship>
{
    public void Configure(EntityTypeBuilder<Ship> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(255);
        builder.Property(s => s.ImoNumber).HasMaxLength(100);
        builder.Property(s => s.RegisterNo).HasMaxLength(100);
        builder.Property(s => s.Longitude).HasMaxLength(20);
        builder.Property(s => s.Latitude).HasMaxLength(20);
        builder.HasOne(s => s.Account)
            .WithOne(a => a.Ship)
            .HasForeignKey<Ship>(s => s.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}