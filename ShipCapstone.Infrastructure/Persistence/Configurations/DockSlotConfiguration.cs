using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class DockSlotConfiguration : IEntityTypeConfiguration<DockSlot>
{
    public void Configure(EntityTypeBuilder<DockSlot> builder)
    {
        builder.HasKey(ds => ds.Id);
        builder.Property(ds => ds.Name).IsRequired().HasMaxLength(255);
        builder.Property(ds => ds.AssignedFrom).IsRequired();
        builder.Property(ds => ds.IsActive).IsRequired();
        builder.HasOne(ds => ds.Boatyard)
            .WithMany(b => b.DockSlots)
            .HasForeignKey(ds => ds.BoatyardId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}