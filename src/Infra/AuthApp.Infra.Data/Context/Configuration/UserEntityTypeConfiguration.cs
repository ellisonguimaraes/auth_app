using AuthApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthApp.Infra.Data.Context.Configuration;

/// <summary>
/// Configuration User Entity Table
/// </summary>
public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(e => e.Name).HasColumnName("Name").HasMaxLength(300).IsRequired();

        builder.Property(e => e.Document).HasColumnName("Document").HasMaxLength(20).IsRequired();

        builder.Property(e => e.PersonId).HasColumnName("PersonId");

        builder.Property(e => e.DocumentType).HasColumnName("DocumentType").HasMaxLength(20).IsRequired();

        builder.Property(e => e.UserType).HasColumnName("UserType").HasMaxLength(20).IsRequired();

        builder.Property(e => e.CreatedAt).HasColumnName("CreatedAt").IsRequired();

        builder.Property(e => e.EditedAt).HasColumnName("EditedAt").IsRequired();

        builder.Property(e => e.HasCompleteRegister).HasColumnName("HasCompleteRegister").IsRequired();

        builder.HasIndex(e => e.PersonId).IsUnique();

        builder.HasIndex(e => new { e.Document, e.DocumentType }).IsUnique();

        builder.Ignore(e => e.Roles);
    }
}
