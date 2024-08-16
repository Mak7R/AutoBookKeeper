using AutoBookKeeper.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static AutoBookKeeper.Core.Rules.UserTokenRules;

namespace AutoBookKeeper.Infrastructure.Configurations;

public class UserTokenEntityConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable("UserToken");

        builder.Property(t => t.Token).HasMaxLength(MaxTokenLength);

        builder
            .HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId);
        
        builder.HasIndex(t => t.UserId);
        builder.HasIndex(t => t.ExpirationTime);
    }
}