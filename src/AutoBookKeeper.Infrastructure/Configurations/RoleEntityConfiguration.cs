using AutoBookKeeper.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoBookKeeper.Infrastructure.Configurations;

public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("UserBookRole");

        builder.HasIndex(r => new { r.Name, r.BookId });
    }
}