using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Infrastructure.SqlDefaults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static AutoBookKeeper.Core.Rules.TransactionTypeRules;

namespace AutoBookKeeper.Infrastructure.Configurations;

public class TransactionTypeEntityConfiguration : IEntityTypeConfiguration<TransactionType>
{
    public void Configure(EntityTypeBuilder<TransactionType> builder)
    {
        builder.ToTable("TransactionType", t =>
        {
            t.HasMinLengthCheckConstraint(nameof(TransactionType.Name), MinNameLength);
        });

        builder.Property(tt => tt.Name).HasMaxLength(MaxNameLength);
        builder.Property(tt => tt.Description).HasMaxLength(MaxDescriptionLength);

        builder
            .HasOne(tt => tt.Book)
            .WithMany()
            .HasForeignKey(tt => tt.BookId);

        builder
            .HasIndex(tt => new { tt.BookId, tt.Name })
            .IsUnique();
    }
}