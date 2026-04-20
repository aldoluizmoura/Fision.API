using FIsionAPI.API.Authentication.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FIsionAPI.API.Authentication;

/// <summary>
/// Responsável por semear Roles padrão e o usuário administrador inicial.
/// </summary>
public static class IdentityDataSeeder
{
    public static async Task SeedAsync(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var sp = scope.ServiceProvider;

        var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("IdentityDataSeeder");
        var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = sp.GetRequiredService<UserManager<User>>();
        var configuration = sp.GetRequiredService<IConfiguration>();

        await SeedRolesAsync(roleManager, logger);
        await SeedAdminAsync(userManager, configuration, logger);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, ILogger logger)
    {
        foreach (var roleName in Roles.Todas)
        {
            if (await roleManager.RoleExistsAsync(roleName)) continue;

            var result = await roleManager.CreateAsync(new IdentityRole(roleName));

            if (result.Succeeded)
                logger.LogInformation("Role '{Role}' criada.", roleName);
            else
                logger.LogWarning("Falha ao criar role '{Role}': {Errors}", roleName,
                    string.Join("; ", result.Errors));
        }
    }

    private static async Task SeedAdminAsync(UserManager<User> userManager,
                                             IConfiguration configuration,
                                             ILogger logger)
    {
        var settings = configuration.GetSection(AdminSeedSettings.SectionName).Get<AdminSeedSettings>();

        if (settings is null
            || string.IsNullOrWhiteSpace(settings.Email)
            || string.IsNullOrWhiteSpace(settings.Senha))
        {
            logger.LogInformation("Seed do admin ignorado: seção 'AdminSeed' não configurada.");
            return;
        }

        var existente = await userManager.FindByEmailAsync(settings.Email);
        if (existente is not null)
        {
            if (!await userManager.IsInRoleAsync(existente, Roles.Admin))
                await userManager.AddToRoleAsync(existente, Roles.Admin);
            return;
        }

        var admin = new User
        {
            UserName = settings.Email,
            Email = settings.Email,
            Nome = settings.Nome,
            EmailConfirmed = true,
            Ativo = true,
            DataCadastro = DateTime.UtcNow
        };

        var created = await userManager.CreateAsync(admin, settings.Senha);
        if (!created.Succeeded)
        {
            logger.LogError("Falha ao criar usuário admin: {Errors}",
                string.Join("; ", created.Errors));
            return;
        }

        await userManager.AddToRoleAsync(admin, Roles.Admin);
        logger.LogInformation("Usuário administrador '{Email}' criado.", settings.Email);
    }
}
