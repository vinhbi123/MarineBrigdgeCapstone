using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class BoatyardConfiguration : IEntityTypeConfiguration<Boatyard>
{
    public void Configure(EntityTypeBuilder<Boatyard> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Name).IsRequired().HasMaxLength(255);
        builder.Property(b => b.Longitude).HasMaxLength(50);
        builder.Property(b => b.Latitude).HasMaxLength(50);
        builder.HasOne(b => b.Account)
            .WithOne(a => a.Boatyard)
            .HasForeignKey<Boatyard>(b => b.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}