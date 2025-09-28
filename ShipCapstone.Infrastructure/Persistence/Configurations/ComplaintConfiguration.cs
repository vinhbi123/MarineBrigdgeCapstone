using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class ComplaintConfiguration : IEntityTypeConfiguration<Complaint>
{
    public void Configure(EntityTypeBuilder<Complaint> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Content).IsRequired().HasMaxLength(1000);
        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (EComplaintStatus)Enum.Parse(typeof(EComplaintStatus), v)
            );
        builder.HasOne(c => c.Account)
            .WithMany(a => a.Complaints)
            .HasForeignKey(c => c.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(c => c.Order)
            .WithMany(o => o.Complaints)
            .HasForeignKey(c => c.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(c => c.Booking)
            .WithMany(b => b.Complaints)
            .HasForeignKey(c => c.BookingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}