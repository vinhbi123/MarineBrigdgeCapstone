using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Status)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (EBookingStatus)Enum.Parse(typeof(EBookingStatus), v)
            );
        builder.Property(b => b.TotalAmount)
            .IsRequired()
            .HasPrecision(12, 2);
        builder.Property(b => b.Type)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (EBookingType)Enum.Parse(typeof(EBookingType), v)
            );
        builder.Property(b => b.StartTime).IsRequired();
        builder.HasOne(b => b.Account)
            .WithMany(a => a.Bookings)
            .HasForeignKey(b => b.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(b => b.DockSlot)
            .WithMany(ds => ds.Bookings)
            .HasForeignKey(b => b.DockSlotId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}