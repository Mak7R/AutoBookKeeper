using AutoBookKeeper.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoBookKeeper.Infrastructure.Configurations;

public class TransactionEntityConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transaction");

        builder
            .HasIndex(t => t.NameIdentifier)
            .IsUnique();
    }
}