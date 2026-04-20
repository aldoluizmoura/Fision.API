using FIsionAPI.API.Authentication.Models;
using System.Threading.Tasks;

namespace FIsionAPI.API.Authentication.Jwt;

public interface ITokenService
{
    /// <summary>
    /// Gera um JWT contendo as claims e roles do usuário informado.
    /// </summary>
    Task<TokenResponse> GerarTokenAsync(User user);
}

public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; }
    public System.DateTime ExpiresAt { get; set; }
}
