using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class ShipPortHistoryConfiguration : IEntityTypeConfiguration<ShipPortHistory>
{
    public void Configure(EntityTypeBuilder<ShipPortHistory> builder)
    {
        builder.HasKey(sph => sph.Id);
        builder.Property(sph => sph.StartDate).IsRequired();
        builder.Property(sph => sph.Status).IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (EShipPortHistoryStatus)Enum.Parse(typeof(EShipPortHistoryStatus), v)
            );
        builder.HasOne(sph => sph.Port)
            .WithMany(p => p.ShipPortHistories)
            .HasForeignKey(sph => sph.PortId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(sph => sph.Ship)
            .WithMany(s => s.ShipPortHistories)
            .HasForeignKey(sph => sph.ShipId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}