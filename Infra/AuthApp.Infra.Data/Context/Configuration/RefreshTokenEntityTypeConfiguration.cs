using AuthApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthApp.Infra.Data.Context.Configuration;

/// <summary>
/// Configuration RefreshToken Entity Table
/// </summary>
public class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshToken");

        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Id)
            .HasColumnName("Id")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(rt => rt.ExpiresIn).HasColumnName("ExpiresIn").IsRequired();

        builder.Property(rt => rt.IsActive).HasColumnName("IsActive").IsRequired();

        builder.Property(e => e.CreatedAt).HasColumnName("CreatedAt").IsRequired();

        builder.Property(e => e.EditedAt).HasColumnName("EditedAt").IsRequired();

        builder.HasIndex(rt => rt.Token).IsUnique();
        builder.Property(rt => rt.Token).HasColumnName("Token").HasMaxLength(450).IsRequired();

        builder.Property(rt => rt.UserId).HasColumnName("UserId").HasMaxLength(450).IsRequired();
        builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId);
    }
}
