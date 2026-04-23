using FIsionAPI.API.Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FIsionAPI.API.Authentication.AuthenticationConfig;

public class AuthenticationDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(entity =>
        {
            entity.ToTable("Usuarios");

            entity.Property(u => u.Nome)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(u => u.Documento)
                  .HasMaxLength(20);

            entity.Property(u => u.DataCadastro)
                  .IsRequired();

            entity.Property(u => u.Ativo)
                  .IsRequired();

            entity.HasIndex(u => u.Documento)
                  .IsUnique()
                  .HasFilter("[Documento] IS NOT NULL");
        });

        builder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshTokens");
            entity.HasKey(rt => rt.Id);
            entity.Property(rt => rt.Token).IsRequired().HasMaxLength(200);
            entity.Property(rt => rt.UserId).IsRequired();
            entity.Property(rt => rt.ExpiresAt).IsRequired();
            entity.Property(rt => rt.CreatedAt).IsRequired();
            entity.HasOne(rt => rt.User)
                  .WithMany()
                  .HasForeignKey(rt => rt.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<IdentityRole>().ToTable("Perfis");

        builder.Entity<IdentityUserRole<string>>()
            .ToTable("UsuariosPerfis");

        builder.Entity<IdentityUserClaim<string>>()
            .ToTable("UsuariosClaims");

        builder.Entity<IdentityUserLogin<string>>()
            .ToTable("UsuariosLogins");

        builder.Entity<IdentityRoleClaim<string>>()
            .ToTable("PerfisClaims");

        builder.Entity<IdentityUserToken<string>>()
            .ToTable("UsuariosTokens");
    }
}
