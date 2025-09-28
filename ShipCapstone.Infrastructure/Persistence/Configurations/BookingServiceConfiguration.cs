using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class BookingServiceConfiguration : IEntityTypeConfiguration<BookingService>
{
    public void Configure(EntityTypeBuilder<BookingService> builder)
    {
        builder.HasKey(bs => bs.Id);
        builder.Property(bs => bs.Status)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (EBookingServiceStatus)Enum.Parse(typeof(EBookingServiceStatus), v)
            );
        builder.HasOne(bs => bs.Booking)
            .WithMany(b => b.BookingServices)
            .HasForeignKey(bs => bs.BookingId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(bs => bs.BoatyardService)
            .WithMany(bs => bs.BookingServices)
            .HasForeignKey(bs => bs.BoatyardServiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}