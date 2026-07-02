namespace FIsionAPI.API.Authentication;

/// <summary>
/// Configurações do usuário administrador inicial (seed).
/// Deve ser definido em appsettings/user-secrets na seção "AdminSeed".
/// </summary>
public class AdminSeedSettings
{
    public const string SectionName = "AdminSeed";

    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Nome { get; set; } = "Administrador";
}
