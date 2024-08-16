using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Infrastructure.SqlDefaults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static AutoBookKeeper.Core.Rules.BookRules;

namespace AutoBookKeeper.Infrastructure.Configurations;

public class BookEntityConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Book", t =>
        {
            t.HasMinLengthCheckConstraint(nameof(Book.Title), MinTitleLength);
        });

        builder.Property(b => b.Title).HasMaxLength(MaxTitleLength);
        builder.Property(b => b.Description).HasMaxLength(MaxDescriptionLength);

        builder
            .HasOne(b => b.Owner)
            .WithMany()
            .HasForeignKey(b => b.OwnerId);
        
        builder.HasIndex(b => b.Title);
    }
}