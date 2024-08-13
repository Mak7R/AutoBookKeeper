using AutoBookKeeper.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoBookKeeper.Infrastructure.Configurations;

public class TransactionTypeEntityConfiguration : IEntityTypeConfiguration<TransactionType>
{
    public void Configure(EntityTypeBuilder<TransactionType> builder)
    {
        builder.ToTable("TransactionType");

        builder
            .HasIndex(tt => new { tt.BookId, tt.Name })
            .IsUnique();
    }
}