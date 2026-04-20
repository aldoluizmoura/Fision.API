namespace FIsionAPI.API.Authentication.Jwt;

/// <summary>
/// Configurações do JWT carregadas da seção "JwtSettings" do appsettings.
/// </summary>
public class JwtSettings
{
    public const string SectionName = "JwtSettings";

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>Duração do access token em minutos.</summary>
    public int ExpirationMinutes { get; set; } = 60;

    /// <summary>Duração do refresh token em dias.</summary>
    public int RefreshTokenExpirationDays { get; set; } = 7;
}
