using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class ModifierOptionConfiguration : IEntityTypeConfiguration<ModifierOption>
{
    public void Configure(EntityTypeBuilder<ModifierOption> builder)
    {
        builder.HasKey(mo => mo.Id);
        builder.Property(mo => mo.Name).IsRequired().HasMaxLength(255);
        builder.Property(mo => mo.DisplayOrder).IsRequired();
        builder.HasOne(mo => mo.ModifierGroup)
            .WithMany(mg => mg.ModifierOptions)
            .HasForeignKey(mo => mo.ModifierGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}