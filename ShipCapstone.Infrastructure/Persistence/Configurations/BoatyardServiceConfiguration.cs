using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class BoatyardServiceConfiguration : IEntityTypeConfiguration<BoatyardService>
{
    public void Configure(EntityTypeBuilder<BoatyardService> builder)
    {
        builder.HasKey(bs => bs.Id);
        builder.Property(bs => bs.TypeService).IsRequired().HasMaxLength(100);
        builder.Property(bs => bs.IsActive).IsRequired();
        builder.Property(bs => bs.Price).IsRequired().HasPrecision(12,2);
        builder.HasOne(bs => bs.Boatyard)
            .WithMany(b => b.BoatyardServices)
            .HasForeignKey(bs => bs.BoatyardId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}