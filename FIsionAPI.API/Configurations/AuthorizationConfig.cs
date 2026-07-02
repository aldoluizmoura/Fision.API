using FIsionAPI.API.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace FIsionAPI.API.Configurations;

public static class AuthorizationConfig
{
    public static IServiceCollection AddAuthorizationConfiguration(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // Fallback: todo endpoint exige autenticação por padrão,
            // a menos que esteja marcado com [AllowAnonymous].
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy(Policies.RequerAdmin, policy =>
                policy.RequireAuthenticatedUser()
                      .RequireRole(Roles.Admin));

            options.AddPolicy(Policies.RequerGestor, policy =>
                policy.RequireAuthenticatedUser()
                      .RequireRole(Roles.Admin, Roles.Gestor));

            options.AddPolicy(Policies.RequerUsuario, policy =>
                policy.RequireAuthenticatedUser()
                      .RequireRole(Roles.Admin, Roles.Gestor, Roles.Usuario));
        });

        return services;
    }
}
