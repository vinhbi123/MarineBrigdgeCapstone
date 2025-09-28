using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class BoatyardReviewConfiguration : IEntityTypeConfiguration<BoatyardReview>
{
    public void Configure(EntityTypeBuilder<BoatyardReview> builder)
    {
        builder.HasKey(br => br.Id);
        builder.Property(br => br.Rating).IsRequired();
        builder.Property(br => br.Comment).HasMaxLength(500);
        builder.HasOne(br => br.Booking)
            .WithMany(b => b.BoatyardReviews)
            .HasForeignKey(br => br.BookingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}