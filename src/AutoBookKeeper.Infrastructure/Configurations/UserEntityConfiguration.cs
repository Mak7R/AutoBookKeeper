using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Infrastructure.SqlDefaults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static AutoBookKeeper.Core.Rules.UserRules;

namespace AutoBookKeeper.Infrastructure.Configurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("ApplicationUser", t =>
        {
            t.HasMinLengthCheckConstraint(nameof(User.UserName), MinUserNameLength);
        });

        builder.Property(u => u.UserName).HasMaxLength(MaxUserNameLength);
        builder.Property(u => u.Email).HasMaxLength(MaxEmailLength);
        builder.Property(u => u.PasswordHash).HasMaxLength(MaxPasswordHashLength);
        
        builder.HasIndex(u => u.UserName).IsUnique();
    }
}