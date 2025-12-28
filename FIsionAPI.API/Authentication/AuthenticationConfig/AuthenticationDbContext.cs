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

        builder.Entity<User>().ToTable("Usuarios");
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
