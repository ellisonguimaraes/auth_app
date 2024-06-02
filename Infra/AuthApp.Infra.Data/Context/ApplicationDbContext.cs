using AuthApp.Domain;
using AuthApp.Infra.Data.Context.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Infra.Data.Context;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        new UserEntityTypeConfiguration().Configure(builder.Entity<User>());
        new RefreshTokenEntityTypeConfiguration().Configure(builder.Entity<RefreshToken>());
        base.OnModelCreating(builder);
    }
}
