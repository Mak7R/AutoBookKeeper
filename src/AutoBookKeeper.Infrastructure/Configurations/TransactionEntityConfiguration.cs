using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Infrastructure.SqlDefaults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static AutoBookKeeper.Core.Rules.TransactionRules;

namespace AutoBookKeeper.Infrastructure.Configurations;

public class TransactionEntityConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transaction", 
            t =>
            {
                t.HasMinLengthCheckConstraint(nameof(Transaction.NameIdentifier), MinNameIdentifierLength);
            });

        builder.Property(t => t.NameIdentifier).HasMaxLength(MaxNameIdentifierLength);
        builder.Property(t => t.Description).HasMaxLength(MaxDescriptionLength);

        builder
            .HasOne(t => t.Book)
            .WithMany()
            .HasForeignKey(t => t.BookId);
        
        builder.HasIndex(t => t.NameIdentifier).IsUnique();
    }
}