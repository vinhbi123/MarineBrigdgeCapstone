using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Title).IsRequired().HasMaxLength(255);
        builder.Property(n => n.IsRead).IsRequired();
        builder.Property(n => n.Content).HasMaxLength(500);
        builder.HasOne(n => n.Account)
            .WithMany(a => a.Notifications)
            .HasForeignKey(n => n.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}