using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Infrastructure.SqlDefaults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static AutoBookKeeper.Core.Rules.BookRoleRules;

namespace AutoBookKeeper.Infrastructure.Configurations;

public class BookRoleEntityConfiguration : IEntityTypeConfiguration<BookRole>
{
    public void Configure(EntityTypeBuilder<BookRole> builder)
    {
        builder.ToTable("BookRole", t =>
        {
            t.HasMinLengthCheckConstraint(nameof(BookRole.Name), MaxNameLength);
        });

        builder.Property(br => br.Name).HasMaxLength(MaxNameLength);

        builder.HasOne(br => br.Book)
            .WithMany()
            .HasForeignKey(br => br.BookId);

        builder.HasIndex(r => new { r.Name, r.BookId });
    }
}