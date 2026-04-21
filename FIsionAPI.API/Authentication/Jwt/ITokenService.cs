using FIsionAPI.API.Authentication.Models;
using System.Threading.Tasks;

namespace FIsionAPI.API.Authentication.Jwt;

public interface ITokenService
{
    /// <summary>
    /// Gera um JWT contendo as claims e roles do usuário informado.
    /// </summary>
    Task<TokenResponse> GerarTokenAsync(User user);

    /// <summary>
    /// Gera e persiste um refresh token para o usuário.
    /// </summary>
    Task<RefreshToken> GerarRefreshTokenAsync(User user);

    /// <summary>
    /// Valida e retorna o refresh token ativo, se existir.
    /// </summary>
    Task<RefreshToken> ObterRefreshTokenAsync(string token);

    /// <summary>
    /// Revoga o refresh token informado.
    /// </summary>
    Task RevogarRefreshTokenAsync(string token);
}

public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; }
    public System.DateTime ExpiresAt { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public System.DateTime RefreshTokenExpiresAt { get; set; }
}
