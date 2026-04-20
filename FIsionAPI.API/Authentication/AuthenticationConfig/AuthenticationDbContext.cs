using FIsionAPI.API.Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FIsionAPI.API.Authentication.AuthenticationConfig;

public class AuthenticationDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
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
