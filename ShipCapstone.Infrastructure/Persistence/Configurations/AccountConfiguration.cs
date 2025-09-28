using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.FullName).IsRequired().HasMaxLength(255);
        builder.HasIndex(a => a.Email)
            .IsUnique();
        builder.Property(a => a.Email).IsRequired().HasMaxLength(255);
        builder.Property(a => a.PasswordHash).IsRequired();
        builder.Property(a => a.Address).HasMaxLength(500);
        builder.Property(a => a.PhoneNumber).HasMaxLength(15);
        builder.Property(a => a.Role)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (ERole)Enum.Parse(typeof(ERole), v)
            );
    }
}