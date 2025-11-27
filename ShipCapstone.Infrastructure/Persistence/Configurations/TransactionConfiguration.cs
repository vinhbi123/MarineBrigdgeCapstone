using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Amount).IsRequired().HasPrecision(12, 2);
        builder.Property(t => t.TransactionCode)
            .IsRequired();
        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (ETransactionStatus)Enum.Parse(typeof(ETransactionStatus), v)
            );
        builder.Property(t => t.Type)
           .IsRequired()
           .HasConversion(
               v => v.ToString(),
               v => (EPaymentType)Enum.Parse(typeof(EPaymentType), v)
           );
        builder.HasOne(t => t.Order)
            .WithMany(o => o.Transactions)
            .HasForeignKey(t => t.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(t => t.Booking)
            .WithMany(b => b.Transactions)
            .HasForeignKey(t => t.BookingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}