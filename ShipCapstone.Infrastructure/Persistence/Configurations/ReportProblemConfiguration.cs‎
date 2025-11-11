using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Infrastructure.Persistence.Configurations;

public class ReportProblemConfiguration : IEntityTypeConfiguration<ReportProblem>
{
    public void Configure(EntityTypeBuilder<ReportProblem> builder)
    {
        builder.HasKey(rp => rp.Id);
        builder.Property(rp => rp.Title)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(rp => rp.Description)
            .IsRequired()
            .HasMaxLength(1000);
        builder.HasOne(rp => rp.Ship)
            .WithMany(s => s.ReportProblems)
            .HasForeignKey(rp => rp.ShipId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(rp => rp.Port)
            .WithMany(p => p.ReportProblems)
            .HasForeignKey(rp => rp.PortId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(rp => rp.Status)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (EReportProblemStatus)Enum.Parse(typeof(EReportProblemStatus), v)
            );
    }
}